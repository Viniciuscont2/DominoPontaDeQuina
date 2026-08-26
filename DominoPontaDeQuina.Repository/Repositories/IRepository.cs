namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Contrato genérico de repositório, com as operações básicas de acesso a dados.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade gerenciada pelo repositório.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> ObterPorIdAsync(Guid id);
    Task<List<TEntity>> ObterTodosAsync();
    Task AdicionarAsync(TEntity entity);
    void Atualizar(TEntity entity);
    void Remover(TEntity entity);
    Task<int> SalvarAlteracoesAsync();
}
