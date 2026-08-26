using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>
/// Repositório responsável pelas operações de acesso a dados da entidade Jogador.
/// </summary>
/// <param name="context">O contexto do Entity Framework Core utilizado para acesso ao banco.</param>
public class JogadorRepository(DominoDbContext context) : RepositoryBase<Jogador>(context)
{
    /// <summary>
    /// Obtem todos os jogadores associados a um usuario.
    /// </summary>
    public async Task<List<Jogador>> ObterPorUsuarioAsync(Guid usuarioId) =>
        await _dbSet
            .Where(j => j.UsuarioId == usuarioId)
            .ToListAsync();

    /// <summary>
    /// Obtem um jogador com o historico de participacoes e respectivas partidas carregado.
    /// </summary>
    public async Task<Jogador?> ObterComParticipacoesAsync(Guid jogadorId) =>
        await _dbSet
            .Include(j => j.Participacoes)
                .ThenInclude(p => p.Partida)
            .FirstOrDefaultAsync(j => j.Id == jogadorId);

    /// <summary>
    /// Busca jogadores cujo nome de exibicao contenha o termo informado.
    /// </summary>
    public async Task<List<Jogador>> BuscarPorNomeAsync(string termo) =>
        await _dbSet
            .Where(j => EF.Functions.Like(j.NomeExibicao, $"%{termo}%"))
            .ToListAsync();
}
