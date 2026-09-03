namespace DominoPontaDeQuina.Services.Exceptions;

/// <summary>
/// Indica que ja existe um usuario cadastrado com o email informado.
/// </summary>
/// <param name="email">O email que ja esta em uso.</param>
public class EmailJaCadastradoException(string email)
    : DominoServiceException($"Ja existe um usuario cadastrado com o email '{email}'.");
