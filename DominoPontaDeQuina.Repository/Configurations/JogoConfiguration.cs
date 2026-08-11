using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
{
    public void Configure(EntityTypeBuilder<Jogo> construtor)
    {
        construtor.ToTable("Jogos");
        construtor.HasKey(jogo => jogo.Id);
        construtor.Property(jogo => jogo.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        construtor.Property(jogo => jogo.IniciadoEm).IsRequired();
        construtor.HasMany(jogo => jogo.Participacoes).WithOne(participacao => participacao.Jogo)
            .HasForeignKey(participacao => participacao.JogoId).OnDelete(DeleteBehavior.Restrict);
    }
}
