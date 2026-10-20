namespace DominoPontaDeQuina.Domain.Exceptions;

/// <summary>
/// Exceção lançada ao tentar registrar um usuário com um e-mail que já existe.
/// </summary>
/// <param name="email">O e-mail que já está cadastrado.</param>
public class EmailJaCadastradoException(string email)
    : Exception($"O e-mail '{email}' já está cadastrado.")
{
    /// <summary>
    /// Obtém o e-mail que causou o conflito.
    /// </summary>
    public string Email { get; } = email;
}
