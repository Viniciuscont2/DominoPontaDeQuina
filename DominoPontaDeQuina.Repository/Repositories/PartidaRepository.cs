using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados da entidade Partida.
/// </summary>
/// <param name="context">O contexto do Entity Framework Core utilizado para acesso ao banco.</param>
public class PartidaRepository(DominoDbContext context) : RepositoryBase<Partida>(context)
{
    /// <summary>
    /// Obtem uma partida com as participacoes e respectivos jogadores carregados.
    /// </summary>
    public async Task<Partida?> ObterComParticipacoesAsync(Guid partidaId) =>
        await _dbSet
            .Include(p => p.Participacoes)
                .ThenInclude(pp => pp.Jogador)
            .FirstOrDefaultAsync(p => p.Id == partidaId);

    /// <summary>
    /// Obtem as partidas que estao no status informado, das mais recentes para as mais antigas.
    /// </summary>
    public async Task<List<Partida>> ObterPorStatusAsync(StatusPartida status) =>
        await _dbSet
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.IniciadoEm)
            .ToListAsync();

    /// <summary>
    /// Obtem as partidas das quais o jogador informado participou.
    /// </summary>
    public async Task<List<Partida>> ObterPorJogadorAsync(Guid jogadorId) =>
        await _dbSet
            .Where(p => p.Participacoes.Any(pp => pp.JogadorId == jogadorId))
            .OrderByDescending(p => p.IniciadoEm)
            .ToListAsync();
}
