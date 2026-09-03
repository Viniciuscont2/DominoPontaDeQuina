using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Fornece a implementacao padrao das operacoes basicas de persistencia via Entity Framework Core.
/// Os repositories especificos herdam esta base e adicionam apenas as consultas LINQ particulares de cada entidade.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade manipulada pelo repository.</typeparam>
/// <typeparam name="TKey">O tipo da chave primaria da entidade.</typeparam>
/// <param name="contexto">O contexto de acesso a dados injetado pelo container de DI.</param>
public abstract class RepositoryBase<TEntity, TKey>(DominoDbContext contexto) : IRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Obtem o contexto de acesso a dados compartilhado pelo repository.
    /// </summary>
    protected DominoDbContext Contexto { get; } = contexto;

    /// <summary>
    /// Obtem o conjunto de entidades gerenciado por este repository.
    /// </summary>
    protected DbSet<TEntity> DbSet => Contexto.Set<TEntity>();

    /// <inheritdoc />
    public virtual async Task<TEntity?> ObterPorIdAsync(TKey id) =>
        await DbSet.FindAsync(id);

    /// <inheritdoc />
    public virtual async Task<List<TEntity>> ObterTodosAsync() =>
        await DbSet.AsNoTracking().ToListAsync();

    /// <inheritdoc />
    public virtual async Task AdicionarAsync(TEntity entidade)
    {
        ArgumentNullException.ThrowIfNull(entidade);
        await DbSet.AddAsync(entidade);
    }

    /// <inheritdoc />
    public virtual void Atualizar(TEntity entidade)
    {
        ArgumentNullException.ThrowIfNull(entidade);
        DbSet.Update(entidade);
    }

    /// <inheritdoc />
    public virtual void Remover(TEntity entidade)
    {
        ArgumentNullException.ThrowIfNull(entidade);
        DbSet.Remove(entidade);
    }

    /// <inheritdoc />
    public virtual async Task<int> SalvarAlteracoesAsync() =>
        await Contexto.SaveChangesAsync();
}
