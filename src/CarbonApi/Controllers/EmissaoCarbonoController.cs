using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonApi.Controllers;

[ApiController]
[Authorize]
public class EmissaoCarbonoController : ControllerBase
{
    private readonly IEmissaoCarbonoService _emissaoService;

    public EmissaoCarbonoController(IEmissaoCarbonoService emissaoService)
    {
        _emissaoService = emissaoService;
    }

    /// <summary>Listar todas as emissões com paginação</summary>
    [HttpGet("api/v1/emissoes-carbono")]
    [ProducesResponseType(typeof(PagedResponse<EmissaoCarbonoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _emissaoService.ListarAsync(page, pageSize);
        return Ok(response);
    }

    /// <summary>Histórico de emissões por empresa com paginação</summary>
    [HttpGet("api/v1/historico-emissoes/{empresaId:long}")]
    [ProducesResponseType(typeof(PagedResponse<EmissaoCarbonoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> HistoricoPorEmpresa(
        [FromRoute] long empresaId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _emissaoService.ListarPorEmpresaAsync(empresaId, page, pageSize);
        return Ok(response);
    }

    /// <summary>Registrar novo relatório de emissão</summary>
    [HttpPost("api/v1/relatorio-emissao")]
    [Authorize(Roles = "ADMIN,EMPRESA")]
    [ProducesResponseType(typeof(EmissaoCarbonoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Registrar([FromBody] EmissaoCarbonoRequest request)
    {
        var response = await _emissaoService.RegistrarAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}