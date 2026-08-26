namespace DominoPontaDeQuina.Domain.Entities;

public class Jogador
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NomeExibicao { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public ICollection<ParticipacaoPartida> Participacoes { get; set; } = new List<ParticipacaoPartida>();
}
