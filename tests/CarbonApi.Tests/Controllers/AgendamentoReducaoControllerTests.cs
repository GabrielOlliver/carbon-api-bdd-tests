using Xunit;
using CarbonApi.Controllers;
using CarbonApi.Models.Enums;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarbonApi.Tests.Controllers;

public class AgendamentoReducaoControllerTests
{
    private readonly Mock<IAgendamentoReducaoService> _agendamentoServiceMock;
    private readonly AgendamentoReducaoController _controller;

    public AgendamentoReducaoControllerTests()
    {
        _agendamentoServiceMock = new Mock<IAgendamentoReducaoService>();
        _controller = new AgendamentoReducaoController(_agendamentoServiceMock.Object);
    }

    [Fact]
    public async Task Listar_DeveRetornar200_QuandoExistemAgendamentos()
    {
        // Arrange
        var pagedResponse = new PagedResponse<AgendamentoReducaoResponse>
        {
            Data = new List<AgendamentoReducaoResponse>
            {
                new() { Id = 1, EmpresaId = 1, EmpresaNome = "EcoTech Brasil",
                        Titulo = "Redução Frota", MetaReducaoTco2e = 50m,
                        Status = StatusAgendamento.Pendente,
                        DataInicio = DateTime.UtcNow,
                        DataFimPrevista = DateTime.UtcNow.AddMonths(6) }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 1
        };

        _agendamentoServiceMock
            .Setup(s => s.ListarAsync(1, 10))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.Listar(1, 10);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value
            .Should().BeOfType<PagedResponse<AgendamentoReducaoResponse>>().Subject;
        response.TotalRecords.Should().Be(1);
    }

    [Fact]
    public async Task BuscarPorId_DeveRetornar200_QuandoAgendamentoExiste()
    {
        // Arrange
        var agendamentoResponse = new AgendamentoReducaoResponse
        {
            Id = 1,
            EmpresaId = 1,
            Titulo = "Redução Frota",
            Status = StatusAgendamento.Pendente
        };

        _agendamentoServiceMock
            .Setup(s => s.BuscarPorIdAsync(1))
            .ReturnsAsync(agendamentoResponse);

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value
            .Should().BeOfType<AgendamentoReducaoResponse>().Subject;
        response.Id.Should().Be(1);
    }
}