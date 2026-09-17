using System.ComponentModel.DataAnnotations;

namespace DominoPontaDeQuina.Api.Dtos;

/// <summary>
/// Dados necessários para criar uma nova conta de usuário.
/// </summary>
/// <param name="Nome">O nome de exibição do usuário.</param>
/// <param name="Email">O e-mail que será usado para login.</param>
/// <param name="Senha">A senha em texto puro, que será convertida em hash antes de ser persistida.</param>
public record RegistrarRequest(
    [property: Required, MinLength(2)] string Nome,
    [property: Required, EmailAddress] string Email,
    [property: Required, MinLength(6)] string Senha);
