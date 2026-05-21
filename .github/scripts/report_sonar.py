import json
import os
import re
import subprocess
import urllib.parse
import urllib.request
import xml.etree.ElementTree as ET

SONAR_HOST = "https://sonarcloud.io"
TRX_NS = {"trx": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def api_get(path: str, params: dict[str, str], token: str) -> dict:
    query = urllib.parse.urlencode({k: v for k, v in params.items() if v})
    url = f"{SONAR_HOST}{path}?{query}" if query else f"{SONAR_HOST}{path}"
    req = urllib.request.Request(url)
    auth = (token + ":").encode("utf-8")
    import base64

    req.add_header("Authorization", f"Basic {base64.b64encode(auth).decode('ascii')}")

    with urllib.request.urlopen(req) as resp:
        return json.loads(resp.read().decode("utf-8"))


def rule_keys_by_tag(token: str, tag: str, language: str = "cs") -> list[str]:
    page = 1
    page_size = 500
    keys: list[str] = []

    while True:
        data = api_get(
            "/api/rules/search",
            {
                "languages": language,
                "types": "CODE_SMELL",
                "tags": tag,
                "isTemplate": "false",
                "p": str(page),
                "ps": str(page_size),
            },
            token,
        )

        rules = data.get("rules", [])
        keys.extend([rule.get("key", "") for rule in rules if rule.get("key")])

        total = int(data.get("total", 0))
        if page * page_size >= total:
            break
        page += 1

    return keys


def issues_total_by_rules(token: str, project_key: str, pr_number: str, rule_keys: list[str]) -> int:
    if not rule_keys:
        return 0

    total = 0
    chunk_size = 100
    for i in range(0, len(rule_keys), chunk_size):
        chunk = rule_keys[i:i + chunk_size]
        data = api_get(
            "/api/issues/search",
            {
                "componentKeys": project_key,
                "pullRequest": pr_number,
                "types": "CODE_SMELL",
                "languages": "cs",
                "rules": ",".join(chunk),
                "ps": "1",
            },
            token,
        )
        total += int(data.get("total", 0))

    return total


def issues_total_by_tag(token: str, project_key: str, pr_number: str, tag: str) -> int:
    data = api_get(
        "/api/issues/search",
        {
            "componentKeys": project_key,
            "pullRequest": pr_number,
            "types": "CODE_SMELL",
            "languages": "cs",
            "tags": tag,
            "ps": "1",
        },
        token,
    )
    return int(data.get("total", 0))


def quality_gate_status(token: str, project_key: str, pr_number: str) -> str:
    data = api_get(
        "/api/qualitygates/project_status",
        {"projectKey": project_key, "pullRequest": pr_number},
        token,
    )
    return data.get("projectStatus", {}).get("status", "UNKNOWN")


def new_lines(token: str, project_key: str, pr_number: str) -> int:
    data = api_get(
        "/api/measures/component",
        {
            "component": project_key,
            "metricKeys": "new_lines",
            "pullRequest": pr_number,
        },
        token,
    )

    measures = data.get("component", {}).get("measures", [])
    if not measures:
        return 0

    return int(float(measures[0].get("value", "0")))


def changed_lines(base_ref: str) -> int:
    if not base_ref:
        return 0

    subprocess.run(["git", "fetch", "origin", base_ref, "--depth=1"], check=False)
    proc = subprocess.run(
        ["git", "diff", "--numstat", f"origin/{base_ref}...HEAD"],
        check=False,
        capture_output=True,
        text=True,
    )

    total = 0
    for line in proc.stdout.splitlines():
        parts = line.split("\t")
        if len(parts) < 2:
            continue

        added, deleted = parts[0], parts[1]
        if added.isdigit():
            total += int(added)
        if deleted.isdigit():
            total += int(deleted)

    return total


def trx_stats(path: str) -> tuple[int, int, int]:
    if not os.path.exists(path):
        return 0, 0, 0

    root = ET.parse(path).getroot()
    counters = root.find("trx:ResultSummary/trx:Counters", TRX_NS)
    if counters is None:
        return 0, 0, 0

    total = int(counters.attrib.get("total", "0"))
    passed = int(counters.attrib.get("passed", "0"))
    failed = int(counters.attrib.get("failed", "0"))
    return total, passed, failed


def should_zero_score(base_ref: str) -> tuple[bool, list[str]]:
    if not base_ref:
        return False, []

    subprocess.run(["git", "fetch", "origin", base_ref, "--depth=1"], check=False)
    proc = subprocess.run(
        ["git", "diff", "--name-only", f"origin/{base_ref}...HEAD"],
        check=False,
        capture_output=True,
        text=True,
    )

    changed_files = [line.strip() for line in proc.stdout.splitlines() if line.strip()]
    blocked = [
        f for f in changed_files
        if f.startswith("DominoPontaDeQuina.Tests/")
        or f.startswith(".github/workflows/")
        or f.startswith(".github/scripts/")
    ]
    return len(blocked) > 0, blocked


def changed_cs_files(base_ref: str) -> list[str]:
    if not base_ref:
        return []

    subprocess.run(["git", "fetch", "origin", base_ref, "--depth=1"], check=False)
    proc = subprocess.run(
        ["git", "diff", "--name-only", f"origin/{base_ref}...HEAD"],
        check=False,
        capture_output=True,
        text=True,
    )

    files = [line.strip() for line in proc.stdout.splitlines() if line.strip()]
    return [
        f for f in files
        if f.endswith(".cs")
        and not f.endswith(".g.cs")
        and not f.endswith(".designer.cs")
        and "obj/" not in f
        and "bin/" not in f
    ]


def xml_doc_coverage(file_paths: list[str]) -> tuple[int, int]:
    signature_pattern = re.compile(
        r"^\s*(public|protected|internal)\s+"
        r"(?:(?:static|sealed|abstract|virtual|override|partial|readonly|async|unsafe|new)\s+)*"
        r"(?:class|record|struct|interface|enum|delegate|event|[A-Za-z_][\w<>,\[\]\.?\s]*)\s+"
        r"[A-Za-z_][\w]*\s*(?:\(|\{|=>|;|:)"
    )
    param_name_pattern = re.compile(r"<param\s+name\s*=\s*\"([^\"]+)\"", re.IGNORECASE)

    excluded_starts = (
        "using ",
        "namespace ",
        "if ",
        "for ",
        "foreach ",
        "while ",
        "switch ",
        "catch ",
        "lock ",
        "return ",
    )

    required = 0
    documented = 0

    for file_path in file_paths:
        if not os.path.exists(file_path):
            continue

        with open(file_path, "r", encoding="utf-8", errors="ignore") as f:
            lines = f.readlines()

        for i, raw in enumerate(lines):
            line = raw.strip()
            if not line or line.startswith("["):
                continue
            if any(line.startswith(prefix) for prefix in excluded_starts):
                continue
            if not signature_pattern.match(line):
                continue

            required += 1

            signature_lines = [line]
            if "(" in line and ")" not in line:
                k = i + 1
                while k < len(lines):
                    next_line = lines[k].strip()
                    if next_line.startswith("///"):
                        break
                    signature_lines.append(next_line)
                    if ")" in next_line:
                        break
                    if "{" in next_line or "=>" in next_line or ";" in next_line:
                        break
                    k += 1
            signature_line = " ".join(part for part in signature_lines if part)
            j = i - 1
            doc_block: list[str] = []
            while j >= 0:
                prev = lines[j].strip()
                if not prev:
                    j -= 1
                    continue
                if prev.startswith("///"):
                    while j >= 0 and lines[j].strip().startswith("///"):
                        doc_block.append(lines[j].strip())
                        j -= 1
                    break
                break

            doc_text = "\n".join(reversed(doc_block))
            doc_text_lower = doc_text.lower()
            has_summary = "<summary>" in doc_text_lower and "</summary>" in doc_text_lower
            has_inheritdoc = "<inheritdoc" in doc_text_lower

            is_override_member = " override " in f" {signature_line} "
            is_explicit_interface_impl = "." in signature_line and "(" in signature_line
            is_derived_or_implements = ":" in signature_line and any(
                keyword in signature_line for keyword in (" class ", " record ", " interface ")
            )
            inheritdoc_allowed = has_inheritdoc and (
                is_override_member or is_explicit_interface_impl or is_derived_or_implements
            )

            method_like = "(" in signature_line and ")" in signature_line and not any(
                k in signature_line for k in (" class ", " record ", " struct ", " interface ", " delegate ")
            )
            returns_required = method_like and " void " not in f" {signature_line} " and not signature_line.endswith(" void")

            param_names: list[str] = []
            if method_like:
                open_idx = signature_line.find("(")
                close_idx = signature_line.rfind(")")
                if open_idx >= 0 and close_idx > open_idx:
                    params_segment = signature_line[open_idx + 1:close_idx].strip()
                    if params_segment and params_segment != "":
                        raw_params = [p.strip() for p in params_segment.split(",") if p.strip()]
                        for p in raw_params:
                            p_no_default = p.split("=")[0].strip()
                            tokens = [t for t in p_no_default.split() if t and t not in ("ref", "out", "in", "params", "this")]
                            if not tokens:
                                continue
                            name_token = tokens[-1]
                            if name_token.startswith("@"):
                                name_token = name_token[1:]
                            while name_token and name_token[-1] in (']', '?'):
                                name_token = name_token[:-1]
                            if name_token:
                                param_names.append(name_token)

            has_all_params = True
            if method_like and param_names:
                doc_param_names = set(name.lower() for name in param_name_pattern.findall(doc_text))
                for param_name in param_names:
                    if param_name.lower() not in doc_param_names:
                        has_all_params = False
                        break

            has_returns = True
            if returns_required:
                has_returns = "<returns>" in doc_text_lower and "</returns>" in doc_text_lower

            if (has_summary and has_all_params and has_returns) or inheritdoc_allowed:
                documented += 1

    return documented, required


def display_score(raw_score: float) -> float:
    return raw_score / 10.0


def main() -> None:
    token = os.environ["SONAR_TOKEN"]
    project_key = os.environ["SONAR_PROJECT_KEY"]
    pr_number = os.environ.get("PR_NUMBER", "")
    base_ref = os.environ.get("BASE_REF", "")

    zero_score, blocked_files = should_zero_score(base_ref)

    summary_path = os.environ["GITHUB_STEP_SUMMARY"]
    with open(summary_path, "a", encoding="utf-8") as summary:
        summary.write("## Critérios de Avaliação\n\n")

        if zero_score:
            summary.write("### Regra de Zeramento Aplicada\n")
            summary.write("Foram detectadas alterações em projeto de testes e/ou workflows. A pontuação total foi zerada.\n")
            summary.write("Arquivos detectados:\n")
            for file in blocked_files:
                summary.write(f"- `{file}`\n")
            return

        qg_status = quality_gate_status(token, project_key, pr_number)
        convention_rule_keys = rule_keys_by_tag(token, "convention")
        convention_smells = issues_total_by_rules(token, project_key, pr_number, convention_rule_keys)
        changed_cs = changed_cs_files(base_ref)
        xml_documented, xml_required = xml_doc_coverage(changed_cs)

        if convention_smells == 0 and convention_rule_keys:
            convention_smells = issues_total_by_tag(token, project_key, pr_number, "convention")
        total_new_lines = new_lines(token, project_key, pr_number)
        total_changed_lines = changed_lines(base_ref)
        total_scoped_lines = max(total_new_lines, total_changed_lines)

        basic_total, basic_passed, basic_failed = trx_stats("TestResults/basic-tests.trx")
        gap_total, gap_passed, gap_failed = trx_stats("TestResults/gap-tests.trx")
        exception_total, exception_passed, exception_failed = trx_stats("TestResults/exception-tests.trx")

        gap_pass_rate = (gap_passed / gap_total) if gap_total > 0 else 0.0
        exception_pass_rate = (exception_passed / exception_total) if exception_total > 0 else 0.0

        score_pipeline = 50.0 * gap_pass_rate
        if xml_required <= 0:
            score_documentation = 0.0
        else:
            score_documentation = 10.0 * (xml_documented / xml_required)

        if total_scoped_lines <= 0:
            score_convention = 0.0
        else:
            score_convention = max(0.0, 10.0 * (1.0 - (convention_smells / total_scoped_lines)))
        score_exception = 10.0 * exception_pass_rate
        score_services = 0.0

        if zero_score:
            score_pipeline = 0.0
            score_documentation = 0.0
            score_exception = 0.0
            score_services = 0.0
            score_convention = 0.0

        summary.write("| Critério | Peso | Pontuação alcançada | Evidência automática |\n")
        summary.write("|---|---:|---:|---|\n")
        summary.write(
            f"| Pipeline de testes | 50% | **{display_score(score_pipeline):.2f}** | Taxa de aprovação dos testes GAP: **{gap_pass_rate * 100:.2f}%** ({gap_passed}/{gap_total}) |\n"
        )
        summary.write(
            f"| Documentação do código | 10% | **{display_score(score_documentation):.2f}** | Membros C# públicos/protegidos alterados exigindo XML doc = **{xml_required}**, com `///` = **{xml_documented}** |\n"
        )
        summary.write(
            f"| Implementação de exceções customizadas | 10% | **{display_score(score_exception):.2f}** | Taxa de aprovação dos testes `Excecao`: **{exception_pass_rate * 100:.2f}%** ({exception_passed}/{exception_total}) |\n"
        )
        summary.write(
            f"| Aderência às convenções do C# | 10% | **{display_score(score_convention):.2f}** | Sonar `new_lines` = **{total_new_lines}**, diff linhas alteradas = **{total_changed_lines}**, base de cálculo = `max(new_lines, linhas alteradas)` = **{total_scoped_lines}**, issues por regras C# com tag `convention` = **{convention_smells}** |\n"
        )
        summary.write(
            f"| Somatório dos pontos | 80% | **{display_score(score_pipeline + score_documentation + score_exception + score_convention):.2f}** | Pontuação parcial alcançada |\n"
        )
        summary.write("\n")
        summary.write(
            "> Observação: `Criação de serviços e validators organizando a lógica do software` `20%` `0.00` `Avaliação manual (não inferida automaticamente)`.\n"
        )
        summary.write("\n")
        summary.write("### SonarCloud\n")
        summary.write(f"- Quality Gate: **{qg_status}**\n")
        summary.write(f"- Projeto: https://sonarcloud.io/project/overview?id={project_key}\n")


if __name__ == "__main__":
    main()
