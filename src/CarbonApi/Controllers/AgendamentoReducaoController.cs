using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarbonApi.Controllers;

[ApiController]
[Route("api/v1/agendamento-reducao-carbono")]
[Authorize]
public class AgendamentoReducaoController : ControllerBase
{
    private readonly IAgendamentoReducaoService _agendamentoService;

    public AgendamentoReducaoController(IAgendamentoReducaoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    /// <summary>Listar agendamentos com paginação</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<AgendamentoReducaoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _agendamentoService.ListarAsync(page, pageSize);
        return Ok(response);
    }

    /// <summary>Criar novo agendamento de redução</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN,EMPRESA")]
    [ProducesResponseType(typeof(AgendamentoReducaoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Criar([FromBody] AgendamentoReducaoRequest request)
    {
        var response = await _agendamentoService.CriarAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>Atualizar agendamento existente</summary>
    [HttpPut("{id:long}")]
    [Authorize(Roles = "ADMIN,EMPRESA")]
    [ProducesResponseType(typeof(AgendamentoReducaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        [FromRoute] long id,
        [FromBody] AgendamentoReducaoUpdateRequest request)
    {
        var response = await _agendamentoService.AtualizarAsync(id, request);
        return Ok(response);
    }

    /// <summary>Cancelar agendamento</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "ADMIN,EMPRESA")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancelar([FromRoute] long id)
    {
        await _agendamentoService.CancelarAsync(id);
        return NoContent();
    }
    
    /// <summary>Buscar agendamento por ID</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AgendamentoReducaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId([FromRoute] long id)
    {
        var response = await _agendamentoService.BuscarPorIdAsync(id);
        return Ok(response);
    }
}