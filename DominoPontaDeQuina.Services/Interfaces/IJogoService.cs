using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Services.Interfaces;

/// <summary>
/// Orquestra as regras de negocio relacionadas ao ciclo de vida de um jogo:
/// inicio, registro de resultados e finalizacao.
/// </summary>
public interface IJogoService
{
    /// <summary>
    /// Inicia um novo jogo, registrando a participacao de cada jogador informado.
    /// </summary>
    /// <param name="jogadoresIds">Os identificadores dos jogadores participantes (minimo de dois).</param>
    Task<Jogo> IniciarJogoAsync(IEnumerable<Guid> jogadoresIds);

    /// <summary>
    /// Registra o resultado de um jogador em um jogo ja iniciado.
    /// </summary>
    Task RegistrarResultadoAsync(Guid jogoId, Guid jogadorId, int posicao, int pontuacao, bool vencedor);

    /// <summary>
    /// Finaliza um jogo, marcando a data de termino e o status final.
    /// </summary>
    Task FinalizarJogoAsync(Guid jogoId);
}
