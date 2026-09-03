using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

/// <summary>
/// Contexto de acesso a dados do DominoPontaDeQuina.
/// Concentra o mapeamento das entidades persistentes (Usuario, Jogador, Jogo, ParticipacaoJogo)
/// e as regras de Fluent API que evoluem o modelo do banco de dados.
/// </summary>
/// <param name="options">As opcoes de configuracao do contexto, definidas na composicao de dependencias.</param>
public class DominoDbContext(DbContextOptions<DominoDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Obtem o conjunto de usuarios cadastrados na aplicacao.
    /// </summary>
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    /// <summary>
    /// Obtem o conjunto de jogadores (perfis de jogo) associados a usuarios.
    /// </summary>
    public DbSet<Jogador> Jogadores => Set<Jogador>();

    /// <summary>
    /// Obtem o conjunto de jogos registrados para consulta de historico.
    /// </summary>
    public DbSet<Jogo> Jogos => Set<Jogo>();

    /// <summary>
    /// Obtem o conjunto de participacoes que ligam jogadores a jogos.
    /// </summary>
    public DbSet<ParticipacaoJogo> Participacoes => Set<ParticipacaoJogo>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(usuario => usuario.Id);
            entity.Property(usuario => usuario.Nome).IsRequired().HasMaxLength(150);
            entity.Property(usuario => usuario.Email).IsRequired().HasMaxLength(200);
            entity.Property(usuario => usuario.HashSenha).IsRequired();
            entity.HasIndex(usuario => usuario.Email).IsUnique();

            entity.HasMany(usuario => usuario.Jogadores)
                  .WithOne(jogador => jogador.Usuario)
                  .HasForeignKey(jogador => jogador.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Jogador>(entity =>
        {
            entity.HasKey(jogador => jogador.Id);
            entity.Property(jogador => jogador.NomeExibicao).IsRequired().HasMaxLength(100);

            entity.HasMany(jogador => jogador.Participacoes)
                  .WithOne(participacao => participacao.Jogador)
                  .HasForeignKey(participacao => participacao.JogadorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Jogo>(entity =>
        {
            entity.HasKey(jogo => jogo.Id);
            entity.Property(jogo => jogo.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20);

            entity.HasMany(jogo => jogo.Participacoes)
                  .WithOne(participacao => participacao.Jogo)
                  .HasForeignKey(participacao => participacao.JogoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ParticipacaoJogo>(entity =>
        {
            entity.HasKey(participacao => participacao.Id);
            entity.HasIndex(participacao => new { participacao.JogoId, participacao.JogadorId }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}
