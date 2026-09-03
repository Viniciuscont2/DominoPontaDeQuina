using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <inheritdoc cref="IJogadorRepository"/>
public class JogadorRepository(DominoDbContext contexto)
    : RepositoryBase<Jogador, Guid>(contexto), IJogadorRepository
{
    /// <inheritdoc />
    public async Task<List<Jogador>> ObterPorUsuarioAsync(Guid usuarioId) =>
        await Contexto.Jogadores
            .AsNoTracking()
            .Where(jogador => jogador.UsuarioId == usuarioId)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<Jogador?> ObterComParticipacoesAsync(Guid jogadorId) =>
        await Contexto.Jogadores
            .Include(jogador => jogador.Participacoes)
            .AsNoTracking()
            .FirstOrDefaultAsync(jogador => jogador.Id == jogadorId);
}
