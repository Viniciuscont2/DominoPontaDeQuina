using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Define as operações de persistência disponíveis para a entidade <see cref="Usuario"/>.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Busca um usuário pelo e-mail cadastrado.
    /// </summary>
    /// <param name="email">O e-mail a ser pesquisado.</param>
    /// <returns>O usuário encontrado, ou <see langword="null"/> quando não existir.</returns>
    Task<Usuario?> BuscarPorEmailAsync(string email);

    /// <summary>
    /// Busca um usuário pelo identificador.
    /// </summary>
    /// <param name="id">O identificador do usuário.</param>
    /// <returns>O usuário encontrado, ou <see langword="null"/> quando não existir.</returns>
    Task<Usuario?> BuscarPorIdAsync(Guid id);

    /// <summary>
    /// Adiciona um novo usuário ao contexto. É necessário chamar <see cref="SalvarAsync"/>
    /// para persistir a alteração no banco de dados.
    /// </summary>
    /// <param name="usuario">O usuário a ser adicionado.</param>
    Task AdicionarAsync(Usuario usuario);

    /// <summary>
    /// Confirma (salva) as alterações pendentes no contexto.
    /// </summary>
    Task SalvarAsync();
}
