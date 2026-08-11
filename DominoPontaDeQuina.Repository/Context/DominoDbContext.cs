using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

public class DominoDbContext(DbContextOptions<DominoDbContext> opcoes) : DbContext(opcoes)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Jogo> Jogos => Set<Jogo>();
    public DbSet<ParticipacaoJogo> ParticipacoesJogo => Set<ParticipacaoJogo>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.ApplyConfigurationsFromAssembly(typeof(DominoDbContext).Assembly);
        base.OnModelCreating(modelo);
    }
}
