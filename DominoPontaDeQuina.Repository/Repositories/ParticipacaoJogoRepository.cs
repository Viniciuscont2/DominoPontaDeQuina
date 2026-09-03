using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <inheritdoc cref="IParticipacaoJogoRepository"/>
public class ParticipacaoJogoRepository(DominoDbContext contexto)
    : RepositoryBase<ParticipacaoJogo, Guid>(contexto), IParticipacaoJogoRepository
{
    /// <inheritdoc />
    public async Task<List<ParticipacaoJogo>> ObterPorJogoAsync(Guid jogoId) =>
        await Contexto.Participacoes
            .Where(participacao => participacao.JogoId == jogoId)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<List<ParticipacaoJogo>> ObterPorJogadorAsync(Guid jogadorId) =>
        await Contexto.Participacoes
            .AsNoTracking()
            .Where(participacao => participacao.JogadorId == jogadorId)
            .ToListAsync();
}
