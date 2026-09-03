using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Services.Interfaces;

/// <summary>
/// Orquestra as regras de negocio relacionadas ao cadastro e consulta de usuarios.
/// </summary>
public interface IUsuarioService
{
    /// <summary>
    /// Registra um novo usuario, validando unicidade de email e armazenando a senha com hash.
    /// </summary>
    Task<Usuario> RegistrarAsync(string nome, string email, string senha);

    /// <summary>
    /// Obtem um usuario pelo seu identificador.
    /// </summary>
    Task<Usuario?> ObterPorIdAsync(Guid id);
}
