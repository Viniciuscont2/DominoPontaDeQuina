using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

/// <summary>
/// Mapeamento Fluent API da entidade Partida.
/// </summary>
public class PartidaConfiguration : IEntityTypeConfiguration<Partida>
{
    public void Configure(EntityTypeBuilder<Partida> builder)
    {
        builder.ToTable("Partidas");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.IniciadoEm)
            .IsRequired();

        builder.HasMany(p => p.Participacoes)
            .WithOne(pp => pp.Partida)
            .HasForeignKey(pp => pp.PartidaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
