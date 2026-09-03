using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Interfaces;

/// <summary>
/// Define o contrato de persistencia para a entidade <see cref="Jogo"/>.
/// </summary>
public interface IJogoRepository : IRepository<Jogo, Guid>
{
    /// <summary>
    /// Obtem os jogos que estao atualmente no status informado.
    /// </summary>
    Task<List<Jogo>> ObterPorStatusAsync(StatusJogo status);

    /// <summary>
    /// Obtem um jogo com suas participacoes e jogadores associados.
    /// </summary>
    Task<Jogo?> ObterComParticipacoesAsync(Guid jogoId);
}
