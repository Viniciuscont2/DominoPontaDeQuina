using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Interfaces;
using DominoPontaDeQuina.Services.Exceptions;
using DominoPontaDeQuina.Services.Interfaces;

namespace DominoPontaDeQuina.Services;

/// <inheritdoc cref="IJogadorService"/>
/// <param name="jogadorRepository">O repository de jogadores.</param>
/// <param name="usuarioRepository">O repository de usuarios, usado para validar o dono do jogador.</param>
/// <param name="participacaoJogoRepository">O repository de participacoes, usado para consultar historico.</param>
public class JogadorService(
    IJogadorRepository jogadorRepository,
    IUsuarioRepository usuarioRepository,
    IParticipacaoJogoRepository participacaoJogoRepository) : IJogadorService
{
    /// <inheritdoc />
    public async Task<Jogador> CriarJogadorAsync(Guid usuarioId, string nomeExibicao)
    {
        if (string.IsNullOrWhiteSpace(nomeExibicao))
            throw new ArgumentException("O nome de exibicao do jogador e obrigatorio.", nameof(nomeExibicao));

        var usuario = await usuarioRepository.ObterPorIdAsync(usuarioId)
            ?? throw new UsuarioNaoEncontradoException(usuarioId);

        var jogador = new Jogador
        {
            UsuarioId = usuario.Id,
            NomeExibicao = nomeExibicao
        };

        await jogadorRepository.AdicionarAsync(jogador);
        await jogadorRepository.SalvarAlteracoesAsync();

        return jogador;
    }

    /// <inheritdoc />
    public async Task<List<ParticipacaoJogo>> ObterHistoricoAsync(Guid jogadorId) =>
        await participacaoJogoRepository.ObterPorJogadorAsync(jogadorId);
}
