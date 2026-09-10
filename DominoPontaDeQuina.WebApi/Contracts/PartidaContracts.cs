using DominoPontaDeQuina.Domain.Entities;

namespace DominoPontaDeQuina.WebApi.Contracts;

/// <summary>Corpo da requisição para iniciar uma nova partida.</summary>
/// <param name="PontuacaoAlvo">Pontuação necessária para vencer a partida.</param>
public record IniciarPartidaRequest(int PontuacaoAlvo = 50);

/// <summary>Corpo da requisição para registrar um jogador em uma partida.</summary>
/// <param name="Nome">Nome exibido do jogador.</param>
/// <param name="UsuarioId">Identificador opcional da conta de usuário associada.</param>
public record RegistrarJogadorRequest(string Nome, Guid? UsuarioId = null);

/// <summary>Corpo da requisição para registrar um lance em uma partida.</summary>
/// <param name="JogadorId">Identificador do jogador que realizou o lance.</param>
public record RegistrarLanceRequest(Guid JogadorId);

/// <summary>Representação pública de uma partida.</summary>
public record PartidaResponse(Guid Id, int PontuacaoAlvo, string Status, DateTime CriadaEm, int TotalJogadores, int TotalLances)
{
    /// <summary>Cria uma resposta a partir da entidade de domínio.</summary>
    public static PartidaResponse FromEntity(Partida partida) =>
        new(partida.Id, partida.PontuacaoAlvo, partida.Status, partida.CriadaEm, partida.Participacoes.Count, partida.Lances.Count);
}

/// <summary>Representação pública de um jogador.</summary>
public record JogadorResponse(Guid Id, string Nome, Guid? UsuarioId)
{
    /// <summary>Cria uma resposta a partir da entidade de domínio.</summary>
    public static JogadorResponse FromEntity(Jogador jogador) =>
        new(jogador.Id, jogador.Nome, jogador.Usuario?.Id);
}

/// <summary>Representação pública de um lance registrado.</summary>
public record LanceResponse(Guid Id, DateTime Timestamp, Guid PartidaId, Guid JogadorId);

/// <summary>Representação pública de uma posição no ranking.</summary>
public record RankingResponse(Guid JogadorId, string JogadorNome, int Vitorias)
{
    /// <summary>Cria uma resposta a partir da entidade de domínio.</summary>
    public static RankingResponse FromEntity(Ranking ranking) =>
        new(ranking.Jogador.Id, ranking.Jogador.Nome, ranking.Vitorias);
}
