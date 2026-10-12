using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonApi.Controllers;

[ApiController]
[Route("api/v1/empresas-sustentaveis")]
[Authorize]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaService _empresaService;

    public EmpresaController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    /// <summary>Listar empresas com paginação</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<EmpresaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _empresaService.ListarAsync(page, pageSize);
        return Ok(response);
    }

    /// <summary>Buscar empresa por ID</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(EmpresaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId([FromRoute] long id)
    {
        var response = await _empresaService.BuscarPorIdAsync(id);
        return Ok(response);
    }

    /// <summary>Cadastrar nova empresa</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(typeof(EmpresaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] EmpresaRequest request)
    {
        var response = await _empresaService.CriarAsync(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = response.Id }, response);
    }
}