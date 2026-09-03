using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.Services.Interfaces;

/// <summary>
/// Orquestra as regras de negocio relacionadas a criacao e consulta de jogadores.
/// </summary>
public interface IJogadorService
{
    /// <summary>
    /// Cria um novo jogador (perfil de jogo) vinculado a um usuario existente.
    /// </summary>
    Task<Jogador> CriarJogadorAsync(Guid usuarioId, string nomeExibicao);

    /// <summary>
    /// Obtem o historico de participacoes de um jogador em jogos anteriores.
    /// </summary>
    Task<List<ParticipacaoJogo>> ObterHistoricoAsync(Guid jogadorId);
}
