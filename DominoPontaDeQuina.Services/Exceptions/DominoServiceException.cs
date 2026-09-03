namespace DominoPontaDeQuina.Services.Exceptions;

/// <summary>
/// Excecao base para violacoes de regras de negocio identificadas na camada de services.
/// </summary>
/// <param name="mensagem">A mensagem descritiva da violacao de regra.</param>
public abstract class DominoServiceException(string mensagem) : Exception(mensagem);
