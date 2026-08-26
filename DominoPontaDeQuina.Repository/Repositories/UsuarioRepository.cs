using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados da entidade Usuario.
/// </summary>
/// <param name="context">O contexto do Entity Framework Core utilizado para acesso ao banco.</param>
public class UsuarioRepository(DominoDbContext context) : RepositoryBase<Usuario>(context)
{
    /// <summary>
    /// Obtem um usuario pelo endereco de email.
    /// </summary>
    public async Task<Usuario?> ObterPorEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

    /// <summary>
    /// Verifica se ja existe um usuario cadastrado com o email informado.
    /// </summary>
    public async Task<bool> ExisteEmailAsync(string email) =>
        await _dbSet.AnyAsync(u => u.Email == email);

    /// <summary>
    /// Obtem um usuario com os jogadores associados carregados.
    /// </summary>
    public async Task<Usuario?> ObterComJogadoresAsync(Guid usuarioId) =>
        await _dbSet
            .Include(u => u.Jogadores)
            .FirstOrDefaultAsync(u => u.Id == usuarioId);
}
