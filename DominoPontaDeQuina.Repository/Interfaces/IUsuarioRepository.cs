using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Repository.Interfaces;

/// <summary>
/// Define o contrato de persistencia para a entidade <see cref="Usuario"/>.
/// </summary>
public interface IUsuarioRepository : IRepository<Usuario, Guid>
{
    /// <summary>
    /// Obtem um usuario pelo endereco de email cadastrado.
    /// </summary>
    Task<Usuario?> ObterPorEmailAsync(string email);

    /// <summary>
    /// Obtem um usuario e seus jogadores associados.
    /// </summary>
    Task<Usuario?> ObterComJogadoresAsync(Guid usuarioId);
}
