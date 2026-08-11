using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class ParticipacaoJogoConfiguration : IEntityTypeConfiguration<ParticipacaoJogo>
{
    public void Configure(EntityTypeBuilder<ParticipacaoJogo> construtor)
    {
        construtor.ToTable("ParticipacoesJogo");
        construtor.HasKey(participacao => participacao.Id);
        construtor.Property(participacao => participacao.Posicao).IsRequired();
        construtor.Property(participacao => participacao.Pontuacao).IsRequired();
        construtor.Property(participacao => participacao.Vencedor).IsRequired();
        construtor.HasIndex(participacao => new { participacao.JogoId, participacao.JogadorId }).IsUnique();
    }
}
