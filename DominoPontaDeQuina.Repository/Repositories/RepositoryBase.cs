using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Implementação base do repositório genérico, reaproveitada pelos repositórios específicos.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade gerenciada pelo repositório.</typeparam>
/// <param name="context">O contexto do Entity Framework Core utilizado para acesso ao banco.</param>
public abstract class RepositoryBase<TEntity>(DominoDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly DominoDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <inheritdoc />
    public virtual async Task<TEntity?> ObterPorIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    /// <inheritdoc />
    public virtual async Task<List<TEntity>> ObterTodosAsync() =>
        await _dbSet.ToListAsync();

    /// <inheritdoc />
    public virtual async Task AdicionarAsync(TEntity entity) =>
        await _dbSet.AddAsync(entity);

    /// <inheritdoc />
    public virtual void Atualizar(TEntity entity) =>
        _dbSet.Update(entity);

    /// <inheritdoc />
    public virtual void Remover(TEntity entity) =>
        _dbSet.Remove(entity);

    /// <inheritdoc />
    public async Task<int> SalvarAlteracoesAsync() =>
        await _context.SaveChangesAsync();
}
