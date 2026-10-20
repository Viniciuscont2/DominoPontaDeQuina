using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Domain.Exceptions;
using DominoPontaDeQuina.Domain.Interfaces;

namespace DominoPontaDeQuina.Domain.Services;

/// <summary>
/// Regras de negócio para cadastro e consulta de usuários.
/// </summary>
/// <param name="repository">O repositório de usuários.</param>
public class UsuarioService(IUsuarioRepository repository)
{
    /// <summary>
    /// Registra um novo usuário, garantindo que o e-mail ainda não esteja em uso.
    /// </summary>
    /// <param name="nome">O nome do usuário.</param>
    /// <param name="email">O e-mail do usuário.</param>
    /// <param name="hashSenha">O hash da senha.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O usuário criado e salvo.</returns>
    /// <exception cref="ArgumentException">Quando algum dado obrigatório estiver vazio.</exception>
    /// <exception cref="EmailJaCadastradoException">Quando o e-mail já existir.</exception>
    public async Task<Usuario> RegistrarAsync(
        string nome, string email, string hashSenha, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nome);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(hashSenha);

        var emailNormalizado = Normalizar(email);

        var existente = await repository.ObterPorEmailAsync(emailNormalizado, cancellationToken);
        if (existente is not null)
            throw new EmailJaCadastradoException(emailNormalizado);

        var usuario = new Usuario
        {
            Nome = nome.Trim(),
            Email = emailNormalizado,
            HashSenha = hashSenha
        };

        await repository.AdicionarAsync(usuario, cancellationToken);
        return usuario;
    }

    /// <summary>
    /// Consulta um usuário pelo e-mail.
    /// </summary>
    /// <param name="email">O e-mail do usuário.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O usuário, ou <see langword="null"/> quando não existir.</returns>
    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        return repository.ObterPorEmailAsync(Normalizar(email), cancellationToken);
    }

    private static string Normalizar(string email) => email.Trim().ToLowerInvariant();
}
