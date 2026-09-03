using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Interfaces;

/// <summary>
/// Define o contrato de persistencia para a entidade <see cref="Jogador"/>.
/// </summary>
public interface IJogadorRepository : IRepository<Jogador, Guid>
{
    /// <summary>
    /// Obtem os jogadores (perfis de jogo) associados a um usuario.
    /// </summary>
    Task<List<Jogador>> ObterPorUsuarioAsync(Guid usuarioId);

    /// <summary>
    /// Obtem um jogador e seu historico de participacoes em jogos.
    /// </summary>
    Task<Jogador?> ObterComParticipacoesAsync(Guid jogadorId);
}
