using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonApi.Controllers;

[ApiController]
[Authorize]
public class IndicadoresController : ControllerBase
{
    private readonly IIndicadoresService _indicadoresService;

    public IndicadoresController(IIndicadoresService indicadoresService)
    {
        _indicadoresService = indicadoresService;
    }

    /// <summary>Indicadores gerais do sistema ESG</summary>
    [HttpGet("api/v1/indicadores-sustentabilidade")]
    [ProducesResponseType(typeof(IndicadoresResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Indicadores()
    {
        var response = await _indicadoresService.CalcularIndicadoresAsync();
        return Ok(response);
    }

    /// <summary>Impacto estimado de uma ação de mitigação</summary>
    [HttpGet("api/v1/impacto-mitigacao/{id:long}")]
    [ProducesResponseType(typeof(ImpactoMitigacaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ImpactoMitigacao([FromRoute] long id)
    {
        var response = await _indicadoresService.CalcularImpactoAsync(id);
        return Ok(response);
    }
}