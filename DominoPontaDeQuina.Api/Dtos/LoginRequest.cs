using System.ComponentModel.DataAnnotations;

namespace DominoPontaDeQuina.Api.Dtos;

/// <summary>
/// Dados necessários para autenticar um usuário já cadastrado.
/// </summary>
/// <param name="Email">O e-mail cadastrado.</param>
/// <param name="Senha">A senha em texto puro, comparada com o hash armazenado.</param>
public record LoginRequest(
    [property: Required, EmailAddress] string Email,
    [property: Required] string Senha);
