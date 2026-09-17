namespace DominoPontaDeQuina.Api.Options;

/// <summary>
/// Representa as configurações necessárias para emitir e validar tokens JWT.
/// Os valores são lidos da seção "Jwt" do appsettings.json.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Nome da seção usada em appsettings.json.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Chave secreta usada para assinar o token. Deve ter, no mínimo, 32 caracteres.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Emissor do token (normalmente o nome da própria aplicação).
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Público-alvo esperado do token.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Tempo de expiração do token, em minutos.
    /// </summary>
    public int ExpiracaoMinutos { get; set; } = 120;
}
