using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonApi.Controllers;

[ApiController]
[Route("api/v1/compensacao-carbono")]
[Authorize]
public class CompensacaoCarbonoController : ControllerBase
{
    private readonly ICompensacaoCarbonoService _compensacaoService;

    public CompensacaoCarbonoController(ICompensacaoCarbonoService compensacaoService)
    {
        _compensacaoService = compensacaoService;
    }

    /// <summary>Listar compensações com paginação</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CompensacaoCarbonoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _compensacaoService.ListarAsync(page, pageSize);
        return Ok(response);
    }

    /// <summary>Cadastrar nova compensação de carbono</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN,EMPRESA")]
    [ProducesResponseType(typeof(CompensacaoCarbonoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Criar([FromBody] CompensacaoCarbonoRequest request)
    {
        var response = await _compensacaoService.CriarAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}