namespace DominoPontaDeQuina.Api.Dtos;

/// <summary>
/// Resposta retornada após um registro ou login bem-sucedido.
/// </summary>
/// <param name="Token">O token JWT que deve ser enviado no cabeçalho Authorization das próximas requisições.</param>
/// <param name="ExpiraEm">Data e hora (UTC) em que o token expira.</param>
/// <param name="UsuarioId">Identificador do usuário autenticado.</param>
/// <param name="Nome">Nome de exibição do usuário.</param>
/// <param name="Email">E-mail do usuário.</param>
public record AuthResponse(
    string Token,
    DateTime ExpiraEm,
    Guid UsuarioId,
    string Nome,
    string Email);
