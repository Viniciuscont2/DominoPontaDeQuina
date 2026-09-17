using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DominoPontaDeQuina.Migrations;

/// <summary>
/// Fábrica usada pelas ferramentas de design-time do EF Core (dotnet-ef) para criar o
/// <see cref="DominoDbContext"/> fora do pipeline normal de injeção de dependência.
/// </summary>
public class DominoDbContextFactory : IDesignTimeDbContextFactory<DominoDbContext>
{
    /// <summary>
    /// Cria o contexto usando SQLite com o arquivo local "domino.db".
    /// Ajuste a connection string aqui caso queira apontar para outro banco.
    /// </summary>
    public DominoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DominoDbContext>();
        optionsBuilder.UseSqlite("Data Source=domino.db");

        return new DominoDbContext(optionsBuilder.Options);
    }
}
