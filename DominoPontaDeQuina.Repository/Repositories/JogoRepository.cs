using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <inheritdoc cref="IJogoRepository"/>
public class JogoRepository(DominoDbContext contexto)
    : RepositoryBase<Jogo, Guid>(contexto), IJogoRepository
{
    /// <inheritdoc />
    public async Task<List<Jogo>> ObterPorStatusAsync(StatusJogo status) =>
        await Contexto.Jogos
            .AsNoTracking()
            .Where(jogo => jogo.Status == status)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<Jogo?> ObterComParticipacoesAsync(Guid jogoId) =>
        await Contexto.Jogos
            .Include(jogo => jogo.Participacoes)
                .ThenInclude(participacao => participacao.Jogador)
            .AsNoTracking()
            .FirstOrDefaultAsync(jogo => jogo.Id == jogoId);
}
