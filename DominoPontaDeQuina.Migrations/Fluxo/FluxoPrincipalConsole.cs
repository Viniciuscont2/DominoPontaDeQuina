using DominoPontaDeQuina.Services.Interfaces;

namespace DominoPontaDeQuina.Migrations.Fluxo;

/// <summary>
/// Fluxo principal de demonstracao da aplicacao.
/// Recebe todas as suas dependencias por construtor (nada e instanciado com "new" aqui),
/// permitindo que o container de DI controle o ciclo de vida de repositories, services e do DbContext.
/// </summary>
/// <param name="usuarioService">O service de usuarios.</param>
/// <param name="jogadorService">O service de jogadores.</param>
/// <param name="jogoService">O service de jogos.</param>
public class FluxoPrincipalConsole(
    IUsuarioService usuarioService,
    IJogadorService jogadorService,
    IJogoService jogoService)
{
    /// <summary>
    /// Executa um fluxo de ponta a ponta: cadastra usuario e jogadores, inicia um jogo,
    /// registra resultados, finaliza o jogo e consulta o historico via LINQ nos repositories.
    /// </summary>
    public async Task ExecutarAsync()
    {
        Console.WriteLine("=== DominoPontaDeQuina - Fluxo Principal (DI) ===");
        Console.WriteLine();

        var usuario = await usuarioService.RegistrarAsync(
            nome: "Aluno Teste",
            email: $"aluno.{Guid.NewGuid():N}@teste.com",
            senha: "SenhaForte123");
        Console.WriteLine($"Usuario registrado: {usuario.Nome} <{usuario.Email}>");

        var jogadorA = await jogadorService.CriarJogadorAsync(usuario.Id, "Jogador A");
        var jogadorB = await jogadorService.CriarJogadorAsync(usuario.Id, "Jogador B");
        Console.WriteLine($"Jogadores criados: '{jogadorA.NomeExibicao}' e '{jogadorB.NomeExibicao}'");

        var jogo = await jogoService.IniciarJogoAsync([jogadorA.Id, jogadorB.Id]);
        Console.WriteLine($"Jogo iniciado: {jogo.Id} (status: {jogo.Status})");

        await jogoService.RegistrarResultadoAsync(jogo.Id, jogadorA.Id, posicao: 1, pontuacao: 120, vencedor: true);
        await jogoService.RegistrarResultadoAsync(jogo.Id, jogadorB.Id, posicao: 2, pontuacao: 80, vencedor: false);
        Console.WriteLine("Resultados registrados para ambos os jogadores.");

        await jogoService.FinalizarJogoAsync(jogo.Id);
        Console.WriteLine("Jogo finalizado.");
        Console.WriteLine();

        var historico = await jogadorService.ObterHistoricoAsync(jogadorA.Id);
        Console.WriteLine($"Historico de '{jogadorA.NomeExibicao}' ({historico.Count} participacao(oes)):");
        foreach (var participacao in historico)
        {
            Console.WriteLine(
                $" - Jogo {participacao.JogoId}: posicao {participacao.Posicao}, " +
                $"pontuacao {participacao.Pontuacao}, vencedor: {participacao.Vencedor}");
        }
    }
}
