using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Api.Services;

/// <summary>
/// Define o contrato do serviço responsável por gerar tokens de acesso para os usuários autenticados.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um token JWT assinado para o usuário informado.
    /// </summary>
    /// <param name="usuario">O usuário autenticado.</param>
    /// <returns>O token gerado e a data/hora UTC de expiração.</returns>
    (string Token, DateTime ExpiraEm) GerarToken(Usuario usuario);
}
