using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Interfaces;

/// <summary>
/// Define o contrato de persistencia para a entidade <see cref="ParticipacaoJogo"/>.
/// </summary>
public interface IParticipacaoJogoRepository : IRepository<ParticipacaoJogo, Guid>
{
    /// <summary>
    /// Obtem todas as participacoes registradas para um determinado jogo.
    /// </summary>
    Task<List<ParticipacaoJogo>> ObterPorJogoAsync(Guid jogoId);

    /// <summary>
    /// Obtem o historico de participacoes de um jogador em diferentes jogos.
    /// </summary>
    Task<List<ParticipacaoJogo>> ObterPorJogadorAsync(Guid jogadorId);
}
