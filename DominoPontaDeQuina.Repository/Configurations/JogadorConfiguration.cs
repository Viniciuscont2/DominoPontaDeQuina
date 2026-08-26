using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

/// <summary>
/// Mapeamento Fluent API da entidade Jogador.
/// </summary>
public class JogadorConfiguration : IEntityTypeConfiguration<Jogador>
{
    public void Configure(EntityTypeBuilder<Jogador> builder)
    {
        builder.ToTable("Jogadores");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.NomeExibicao)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(j => j.Participacoes)
            .WithOne(p => p.Jogador)
            .HasForeignKey(p => p.JogadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
