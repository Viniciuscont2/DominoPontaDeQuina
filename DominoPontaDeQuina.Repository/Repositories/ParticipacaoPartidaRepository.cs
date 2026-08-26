using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados da entidade ParticipacaoPartida.
/// </summary>
/// <param name="context">O contexto do Entity Framework Core utilizado para acesso ao banco.</param>
public class ParticipacaoPartidaRepository(DominoDbContext context) : RepositoryBase<ParticipacaoPartida>(context)
{
    /// <summary>
    /// Obtem as participacoes de uma partida, ordenadas pela posicao, com o jogador carregado.
    /// </summary>
    public async Task<List<ParticipacaoPartida>> ObterPorPartidaAsync(Guid partidaId) =>
        await _dbSet
            .Where(pp => pp.PartidaId == partidaId)
            .Include(pp => pp.Jogador)
            .OrderBy(pp => pp.Posicao)
            .ToListAsync();

    /// <summary>
    /// Obtem o total de vitorias de um jogador ao longo do historico de partidas.
    /// </summary>
    public async Task<int> ObterTotalVitoriasAsync(Guid jogadorId) =>
        await _dbSet.CountAsync(pp => pp.JogadorId == jogadorId && pp.Vencedor);

    /// <summary>
    /// Obtem a media de pontuacao de um jogador ao longo do historico de partidas.
    /// </summary>
    public async Task<double> ObterMediaPontuacaoAsync(Guid jogadorId)
    {
        var participacoes = _dbSet.Where(pp => pp.JogadorId == jogadorId);

        return await participacoes.AnyAsync()
            ? await participacoes.AverageAsync(pp => pp.Pontuacao)
            : 0;
    }
}
