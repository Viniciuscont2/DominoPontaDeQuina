using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DominoPontaDeQuina.Migrations;

/// <summary>
/// Fabrica exigida pelas ferramentas de design-time do EF Core (dotnet ef) para criar o
/// <see cref="DominoDbContext"/> fora do fluxo normal de injecao de dependencias da aplicacao.
/// </summary>
public class DominoDbContextFactory : IDesignTimeDbContextFactory<DominoDbContext>
{
    /// <inheritdoc />
    public DominoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DominoDbContext>();
        optionsBuilder.UseSqlite("Data Source=domino.db");

        return new DominoDbContext(optionsBuilder.Options);
    }
}
