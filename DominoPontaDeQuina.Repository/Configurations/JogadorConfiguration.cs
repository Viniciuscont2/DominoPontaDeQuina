using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class JogadorConfiguration : IEntityTypeConfiguration<Jogador>
{
    public void Configure(EntityTypeBuilder<Jogador> construtor)
    {
        construtor.ToTable("Jogadores");
        construtor.HasKey(jogador => jogador.Id);
        construtor.Property(jogador => jogador.NomeExibicao).HasMaxLength(100).IsRequired();
        construtor.HasMany(jogador => jogador.Participacoes).WithOne(participacao => participacao.Jogador)
            .HasForeignKey(participacao => participacao.JogadorId).OnDelete(DeleteBehavior.Restrict);
    }
}
