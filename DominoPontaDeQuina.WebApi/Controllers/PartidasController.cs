using DominoPontaDeQuina.Application.Services;
using DominoPontaDeQuina.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.WebApi.Controllers;

/// <summary>Expõe as operações da camada de aplicação relacionadas a partidas.</summary>
[ApiController]
[Route("api/partidas")]
public class PartidasController(IPartidaService partidaService) : ControllerBase
{
    /// <summary>Inicia uma nova partida.</summary>
    /// <param name="request">Pontuação alvo desejada.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    [HttpPost]
    [ProducesResponseType(typeof(PartidaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PartidaResponse>> IniciarPartida(
        [FromBody] IniciarPartidaRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var partida = await partidaService.IniciarPartidaAsync(request.PontuacaoAlvo, cancellationToken);
            var response = PartidaResponse.FromEntity(partida);
            return CreatedAtAction(nameof(VerificarStatus), new { partidaId = response.Id }, response);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    /// <summary>Consulta o status atual de uma partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    [HttpGet("{partidaId:guid}")]
    [ProducesResponseType(typeof(PartidaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PartidaResponse>> VerificarStatus(
        Guid partidaId,
        CancellationToken cancellationToken)
    {
        try
        {
            var partida = await partidaService.VerificarStatusAsync(partidaId, cancellationToken);
            return Ok(PartidaResponse.FromEntity(partida));
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
    }

    /// <summary>Consulta o histórico de partidas registradas.</summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    [HttpGet("historico")]
    [ProducesResponseType(typeof(IReadOnlyList<PartidaResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PartidaResponse>>> ConsultarHistorico(
        CancellationToken cancellationToken)
    {
        var historico = await partidaService.ConsultarHistoricoAsync(cancellationToken);
        return Ok(historico.Select(PartidaResponse.FromEntity).ToList());
    }

    /// <summary>Registra um jogador e sua participação inicial em uma partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="request">Dados do jogador.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    [HttpPost("{partidaId:guid}/jogadores")]
    [ProducesResponseType(typeof(JogadorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JogadorResponse>> RegistrarJogador(
        Guid partidaId,
        [FromBody] RegistrarJogadorRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var jogador = await partidaService.RegistrarJogadorAsync(partidaId, request.Nome, request.UsuarioId, cancellationToken);
            var response = JogadorResponse.FromEntity(jogador);
            return CreatedAtAction(nameof(VerificarStatus), new { partidaId }, response);
        }
        catch (ArgumentException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
    }

    /// <summary>Registra um lance realizado por um jogador em uma partida.</summary>
    /// <param name="partidaId">Identificador da partida.</param>
    /// <param name="request">Jogador que realizou o lance.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    [HttpPost("{partidaId:guid}/lances")]
    [ProducesResponseType(typeof(LanceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LanceResponse>> RegistrarLance(
        Guid partidaId,
        [FromBody] RegistrarLanceRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var lance = await partidaService.RegistrarLanceAsync(partidaId, request.JogadorId, cancellationToken);
            var response = new LanceResponse(lance.Id, lance.Timestamp, partidaId, request.JogadorId);
            return CreatedAtAction(nameof(VerificarStatus), new { partidaId }, response);
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status409Conflict);
        }
    }
}
