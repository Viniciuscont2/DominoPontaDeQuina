using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DominoPontaDeQuina.Migrations;

public class DominoDbContextFactory : IDesignTimeDbContextFactory<DominoDbContext>
{
    public DominoDbContext CreateDbContext(string[] args)
    {
        var construtor = new DbContextOptionsBuilder<DominoDbContext>();
        construtor.UseSqlite("Data Source=domino.db");
        return new DominoDbContext(construtor.Options);
    }
}
