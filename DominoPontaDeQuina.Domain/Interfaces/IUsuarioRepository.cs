using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Domain.Interfaces;

/// <summary>
/// Contrato de acesso a dados de <see cref="Usuario"/>.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Consulta um usuário pelo e-mail.
    /// </summary>
    /// <param name="email">O e-mail (já normalizado) a ser consultado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O usuário encontrado, ou <see langword="null"/> quando não existir.</returns>
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persiste um novo usuário.
    /// </summary>
    /// <param name="usuario">O usuário a ser salvo.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default);
}
