namespace DominoPontaDeQuina.Services.Exceptions;

/// <summary>
/// Indica que o usuario informado nao foi encontrado.
/// </summary>
/// <param name="usuarioId">O identificador do usuario procurado.</param>
public class UsuarioNaoEncontradoException(Guid usuarioId)
    : DominoServiceException($"Usuario '{usuarioId}' nao foi encontrado.");
