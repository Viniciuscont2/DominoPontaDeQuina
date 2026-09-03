using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Interfaces;
using DominoPontaDeQuina.Services.Exceptions;
using DominoPontaDeQuina.Services.Interfaces;

namespace DominoPontaDeQuina.Services;

/// <inheritdoc cref="IJogoService"/>
/// <param name="jogoRepository">O repository de jogos.</param>
/// <param name="jogadorRepository">O repository de jogadores, usado para validar participantes.</param>
/// <param name="participacaoJogoRepository">O repository de participacoes.</param>
public class JogoService(
    IJogoRepository jogoRepository,
    IJogadorRepository jogadorRepository,
    IParticipacaoJogoRepository participacaoJogoRepository) : IJogoService
{
    /// <inheritdoc />
    public async Task<Jogo> IniciarJogoAsync(IEnumerable<Guid> jogadoresIds)
    {
        var idsDistintos = jogadoresIds.Distinct().ToList();

        if (idsDistintos.Count < 2)
            throw new OperacaoInvalidaException("Um jogo precisa de ao menos dois jogadores.");

        var jogo = new Jogo
        {
            Status = StatusJogo.EmAndamento
        };

        await jogoRepository.AdicionarAsync(jogo);

        foreach (var jogadorId in idsDistintos)
        {
            _ = await jogadorRepository.ObterPorIdAsync(jogadorId)
                ?? throw new JogadorNaoEncontradoException(jogadorId);

            var participacao = new ParticipacaoJogo
            {
                JogoId = jogo.Id,
                Jogo = jogo,
                JogadorId = jogadorId
            };

            await participacaoJogoRepository.AdicionarAsync(participacao);
        }

        await jogoRepository.SalvarAlteracoesAsync();

        return jogo;
    }

    /// <inheritdoc />
    public async Task RegistrarResultadoAsync(Guid jogoId, Guid jogadorId, int posicao, int pontuacao, bool vencedor)
    {
        var participacoes = await participacaoJogoRepository.ObterPorJogoAsync(jogoId);

        var participacao = participacoes.FirstOrDefault(p => p.JogadorId == jogadorId)
            ?? throw new OperacaoInvalidaException("O jogador informado nao participa deste jogo.");

        participacao.Posicao = posicao;
        participacao.Pontuacao = pontuacao;
        participacao.Vencedor = vencedor;

        participacaoJogoRepository.Atualizar(participacao);
        await participacaoJogoRepository.SalvarAlteracoesAsync();
    }

    /// <inheritdoc />
    public async Task FinalizarJogoAsync(Guid jogoId)
    {
        var jogo = await jogoRepository.ObterPorIdAsync(jogoId)
            ?? throw new JogoNaoEncontradoException(jogoId);

        if (jogo.Status is StatusJogo.Finalizado)
            throw new OperacaoInvalidaException("O jogo ja esta finalizado.");

        jogo.Status = StatusJogo.Finalizado;
        jogo.FinalizadoEm = DateTime.UtcNow;

        jogoRepository.Atualizar(jogo);
        await jogoRepository.SalvarAlteracoesAsync();
    }
}
