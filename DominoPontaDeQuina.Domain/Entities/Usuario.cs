using System.ComponentModel.DataAnnotations;

namespace DominoPontaDeQuina.Domain.Entities;

/// <summary>
/// Representa a conta do aplicativo cliente. Um usuario pode possuir varios jogadores (perfis de jogo).
/// </summary>
public class Usuario
{
    /// <summary>
    /// Identificador unico do usuario.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nome completo do usuario.
    /// </summary>
    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// E-mail utilizado para autenticacao e contato.
    /// </summary>
    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash da senha do usuario. A senha em texto puro nunca deve ser armazenada.
    /// </summary>
    [Required]
    public string HashSenha { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora de criacao da conta.
    /// </summary>
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Jogadores (perfis de jogo) associados a este usuario.
    /// </summary>
    public ICollection<Jogador> Jogadores { get; set; } = new List<Jogador>();
}
