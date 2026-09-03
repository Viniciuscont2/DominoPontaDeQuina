using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <inheritdoc cref="IUsuarioRepository"/>
public class UsuarioRepository(DominoDbContext contexto)
    : RepositoryBase<Usuario, Guid>(contexto), IUsuarioRepository
{
    /// <inheritdoc />
    public async Task<Usuario?> ObterPorEmailAsync(string email) =>
        await Contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Email == email);

    /// <inheritdoc />
    public async Task<Usuario?> ObterComJogadoresAsync(Guid usuarioId) =>
        await Contexto.Usuarios
            .Include(usuario => usuario.Jogadores)
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == usuarioId);
}
