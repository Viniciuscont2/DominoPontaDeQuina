using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

/// <summary>
/// Mapeamento Fluent API da entidade ParticipacaoPartida.
/// </summary>
public class ParticipacaoPartidaConfiguration : IEntityTypeConfiguration<ParticipacaoPartida>
{
    public void Configure(EntityTypeBuilder<ParticipacaoPartida> builder)
    {
        builder.ToTable("ParticipacoesPartida");
        builder.HasKey(pp => pp.Id);

        builder.Property(pp => pp.Pontuacao)
            .HasDefaultValue(0);

        builder.Property(pp => pp.Vencedor)
            .HasDefaultValue(false);

        builder.HasIndex(pp => new { pp.PartidaId, pp.JogadorId })
            .IsUnique();
    }
}
