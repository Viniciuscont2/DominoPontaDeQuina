using System.Security.Cryptography;
using System.Text;
using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Interfaces;
using DominoPontaDeQuina.Services.Exceptions;
using DominoPontaDeQuina.Services.Interfaces;

namespace DominoPontaDeQuina.Services;

/// <inheritdoc cref="IUsuarioService"/>
/// <param name="usuarioRepository">O repository de usuarios, injetado pelo container de DI.</param>
public class UsuarioService(IUsuarioRepository usuarioRepository) : IUsuarioService
{
    /// <inheritdoc />
    public async Task<Usuario> RegistrarAsync(string nome, string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do usuario e obrigatorio.", nameof(nome));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O email do usuario e obrigatorio.", nameof(email));
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("A senha do usuario e obrigatoria.", nameof(senha));

        var usuarioExistente = await usuarioRepository.ObterPorEmailAsync(email);
        if (usuarioExistente is not null)
            throw new EmailJaCadastradoException(email);

        var usuario = new Usuario
        {
            Nome = nome,
            Email = email,
            HashSenha = GerarHashSenha(senha)
        };

        await usuarioRepository.AdicionarAsync(usuario);
        await usuarioRepository.SalvarAlteracoesAsync();

        return usuario;
    }

    /// <inheritdoc />
    public async Task<Usuario?> ObterPorIdAsync(Guid id) =>
        await usuarioRepository.ObterPorIdAsync(id);

    /// <summary>
    /// Gera o hash SHA-256 da senha informada, evitando o armazenamento em texto puro.
    /// </summary>
    private static string GerarHashSenha(string senha)
    {
        var bytesHash = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToHexString(bytesHash);
    }
}
