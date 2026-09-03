namespace DominoPontaDeQuina.Repository.Interfaces;

/// <summary>
/// Define as operacoes basicas de persistencia compartilhadas pelos repositories do projeto.
/// Serve como contrato generico reaproveitado pelos repositories especificos de cada entidade.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade manipulada pelo repository.</typeparam>
/// <typeparam name="TKey">O tipo da chave primaria da entidade.</typeparam>
public interface IRepository<TEntity, in TKey> where TEntity : class
{
    /// <summary>
    /// Obtem uma entidade pelo seu identificador.
    /// </summary>
    Task<TEntity?> ObterPorIdAsync(TKey id);

    /// <summary>
    /// Obtem todas as entidades do tipo, sem rastreamento de alteracoes.
    /// </summary>
    Task<List<TEntity>> ObterTodosAsync();

    /// <summary>
    /// Adiciona uma nova entidade ao contexto, pendente de persistencia.
    /// </summary>
    Task AdicionarAsync(TEntity entidade);

    /// <summary>
    /// Marca uma entidade existente como alterada.
    /// </summary>
    void Atualizar(TEntity entidade);

    /// <summary>
    /// Marca uma entidade existente para remocao.
    /// </summary>
    void Remover(TEntity entidade);

    /// <summary>
    /// Persiste no banco de dados as alteracoes pendentes no contexto.
    /// </summary>
    /// <returns>A quantidade de registros afetados.</returns>
    Task<int> SalvarAlteracoesAsync();
}
