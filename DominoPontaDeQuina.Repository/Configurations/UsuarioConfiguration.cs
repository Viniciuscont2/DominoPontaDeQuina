using DominoPontaDeQuina.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominoPontaDeQuina.Repository.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> construtor)
    {
        construtor.ToTable("Usuarios");
        construtor.HasKey(usuario => usuario.Id);
        construtor.Property(usuario => usuario.Nome).HasMaxLength(150).IsRequired();
        construtor.Property(usuario => usuario.Email).HasMaxLength(254).IsRequired();
        construtor.Property(usuario => usuario.HashSenha).HasMaxLength(500).IsRequired();
        construtor.Property(usuario => usuario.CriadoEm).IsRequired();
        construtor.HasIndex(usuario => usuario.Email).IsUnique();
        construtor.HasMany(usuario => usuario.Jogadores).WithOne(jogador => jogador.Usuario)
            .HasForeignKey(jogador => jogador.UsuarioId).OnDelete(DeleteBehavior.Restrict);
    }
}
