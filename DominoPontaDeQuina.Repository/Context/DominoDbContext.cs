using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

/// <summary>
/// Representa a sessão do Entity Framework Core com o banco de dados do DominoPontaDeQuina.
/// </summary>
/// <param name="options">As opções de configuração do contexto (provedor, connection string, etc.).</param>
public class DominoDbContext(DbContextOptions<DominoDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Obtém as contas de usuário persistidas, usadas na autenticação da aplicação.
    /// </summary>
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    /// <summary>
    /// Obtém os perfis de jogador persistidos.
    /// </summary>
    public DbSet<Jogador> Jogadores => Set<Jogador>();

    /// <summary>
    /// Obtém o histórico de jogos persistidos.
    /// </summary>
    public DbSet<Jogo> Jogos => Set<Jogo>();

    /// <summary>
    /// Obtém as participações de jogadores em jogos.
    /// </summary>
    public DbSet<ParticipacaoJogo> Participacoes => Set<ParticipacaoJogo>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(usuario =>
        {
            usuario.ToTable("Usuarios");
            usuario.HasKey(u => u.Id);
            usuario.Property(u => u.Nome).HasMaxLength(150).IsRequired();
            usuario.Property(u => u.Email).HasMaxLength(320).IsRequired();
            usuario.HasIndex(u => u.Email).IsUnique();
            usuario.Property(u => u.HashSenha).HasMaxLength(500).IsRequired();
            usuario.Property(u => u.CriadoEm).IsRequired();

            usuario.HasMany(u => u.Jogadores)
                   .WithOne(j => j.Usuario)
                   .HasForeignKey(j => j.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Jogador>(jogador =>
        {
            jogador.ToTable("Jogadores");
            jogador.HasKey(j => j.Id);
            jogador.Property(j => j.NomeExibicao).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Jogo>(jogo =>
        {
            jogo.ToTable("Jogos");
            jogo.HasKey(j => j.Id);

            jogo.HasMany(j => j.Participacoes)
                .WithOne(p => p.Jogo)
                .HasForeignKey(p => p.JogoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ParticipacaoJogo>(participacao =>
        {
            participacao.ToTable("ParticipacoesJogo");
            participacao.HasKey(p => p.Id);

            participacao.HasOne(p => p.Jogador)
                         .WithMany(j => j.Participacoes)
                         .HasForeignKey(p => p.JogadorId)
                         .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }
}
