namespace DominoPontaDeQuina.Services.Exceptions;

/// <summary>
/// Indica que o jogo informado nao foi encontrado.
/// </summary>
/// <param name="jogoId">O identificador do jogo procurado.</param>
public class JogoNaoEncontradoException(Guid jogoId)
    : DominoServiceException($"Jogo '{jogoId}' nao foi encontrado.");
