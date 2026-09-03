namespace DominoPontaDeQuina.Services.Exceptions;

/// <summary>
/// Indica que a operacao solicitada nao e valida para o estado atual dos dados.
/// </summary>
/// <param name="mensagem">A mensagem descritiva do motivo da invalidade.</param>
public class OperacaoInvalidaException(string mensagem) : DominoServiceException(mensagem);
