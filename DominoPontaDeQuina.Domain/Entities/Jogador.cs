using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominoPontaDeQuina.Domain.Entities;

/// <summary>
/// Representa um perfil de jogo associado a um usuario. Um usuario pode ter varios jogadores.
/// </summary>
public class Jogador
{
    /// <summary>
    /// Identificador unico do jogador.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nome exibido do jogador durante as partidas.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string NomeExibicao { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do usuario dono deste jogador.
    /// </summary>
    [Required]
    public Guid UsuarioId { get; set; }

    /// <summary>
    /// Usuario dono deste jogador.
    /// </summary>
    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; } = null!;

    /// <summary>
    /// Participacoes deste jogador em jogos registrados.
    /// </summary>
    public ICollection<ParticipacaoJogo> Participacoes { get; set; } = new List<ParticipacaoJogo>();
}
