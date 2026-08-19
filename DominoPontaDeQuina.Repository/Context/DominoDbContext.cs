using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Context;

/// <summary>
/// Contexto do Entity Framework Core responsavel pela persistencia das entidades do dominio
/// (Usuario, Jogador, Jogo e ParticipacaoJogo).
/// </summary>
public class DominoDbContext : DbContext
{
    /// <summary>
    /// Construtor sem parametros, usado pela ferramenta de migrations e por
    /// <see cref="DominoPontaDeQuina.Migrations.DominoDbContextFactory"/>.
    /// </summary>
    public DominoDbContext()
    {
    }

    /// <summary>
    /// Construtor que recebe as opcoes de configuracao do contexto (util para testes e injecao de dependencia).
    /// </summary>
    /// <param name="options">As opcoes de configuracao do contexto.</param>
    public DominoDbContext(DbContextOptions<DominoDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Usuarios cadastrados no sistema.
    /// </summary>
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    /// <summary>
    /// Jogadores (perfis de jogo) cadastrados no sistema.
    /// </summary>
    public DbSet<Jogador> Jogadores => Set<Jogador>();

    /// <summary>
    /// Jogos registrados para consulta de historico.
    /// </summary>
    public DbSet<Jogo> Jogos => Set<Jogo>();

    /// <summary>
    /// Participacoes de jogadores em jogos registrados.
    /// </summary>
    public DbSet<ParticipacaoJogo> Participacoes => Set<ParticipacaoJogo>();

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // SQL Server LocalDB, instalado junto com o Visual Studio.
            // Se voce usa outra instancia (ex: SQL Server Express), ajuste a string de conexao abaixo.
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=DominoPontaDeQuinaDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
