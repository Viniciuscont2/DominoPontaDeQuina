namespace DominoPontaDeQuina.Services.Exceptions;

/// <summary>
/// Indica que o jogador informado nao foi encontrado.
/// </summary>
/// <param name="jogadorId">O identificador do jogador procurado.</param>
public class JogadorNaoEncontradoException(Guid jogadorId)
    : DominoServiceException($"Jogador '{jogadorId}' nao foi encontrado.");
