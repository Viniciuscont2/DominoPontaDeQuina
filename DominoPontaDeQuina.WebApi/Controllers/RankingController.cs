using DominoPontaDeQuina.Application.Services;
using DominoPontaDeQuina.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DominoPontaDeQuina.WebApi.Controllers;

/// <summary>Expõe a consulta do ranking de jogadores.</summary>
[ApiController]
[Route("api/ranking")]
public class RankingController(IPartidaService partidaService) : ControllerBase
{
    /// <summary>Consulta o ranking de jogadores por vitórias.</summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RankingResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RankingResponse>>> ConsultarRanking(
        CancellationToken cancellationToken)
    {
        var ranking = await partidaService.ConsultarRankingAsync(cancellationToken);
        return Ok(ranking.Select(RankingResponse.FromEntity).ToList());
    }
}
