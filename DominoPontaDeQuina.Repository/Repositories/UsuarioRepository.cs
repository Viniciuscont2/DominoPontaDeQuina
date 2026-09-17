using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <inheritdoc cref="IUsuarioRepository"/>
public class UsuarioRepository(DominoDbContext context) : IUsuarioRepository
{
    /// <inheritdoc />
    public Task<Usuario?> BuscarPorEmailAsync(string email) =>
        context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == email);

    /// <inheritdoc />
    public Task<Usuario?> BuscarPorIdAsync(Guid id) =>
        context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == id);

    /// <inheritdoc />
    public async Task AdicionarAsync(Usuario usuario) =>
        await context.Usuarios.AddAsync(usuario);

    /// <inheritdoc />
    public Task SalvarAsync() =>
        context.SaveChangesAsync();
}
