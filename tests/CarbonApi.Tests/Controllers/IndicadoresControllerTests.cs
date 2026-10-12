using Xunit;
using CarbonApi.Controllers;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarbonApi.Tests.Controllers;

public class IndicadoresControllerTests
{
    private readonly Mock<IIndicadoresService> _indicadoresServiceMock;
    private readonly IndicadoresController _controller;

    public IndicadoresControllerTests()
    {
        _indicadoresServiceMock = new Mock<IIndicadoresService>();
        _controller = new IndicadoresController(_indicadoresServiceMock.Object);
    }

    [Fact]
    public async Task Indicadores_DeveRetornar200_ComTotaisDoSistema()
    {
        // Arrange
        var indicadoresResponse = new IndicadoresResponse
        {
            TotalEmpresas = 2,
            TotalEmissoes = 5,
            TotalTco2eEmitido = 752.5m,
            TotalAgendamentos = 3,
            AgendamentosConcluidos = 1,
            TotalTco2eCompensado = 30m,
            SaldoLiquidoTco2e = 722.5m
        };

        _indicadoresServiceMock
            .Setup(s => s.CalcularIndicadoresAsync())
            .ReturnsAsync(indicadoresResponse);

        // Act
        var result = await _controller.Indicadores();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value.Should().BeOfType<IndicadoresResponse>().Subject;
        response.TotalEmpresas.Should().Be(2);
        response.SaldoLiquidoTco2e.Should().Be(722.5m);
    }

    [Fact]
    public async Task ImpactoMitigacao_DeveRetornar200_QuandoAgendamentoExiste()
    {
        // Arrange
        var impactoResponse = new ImpactoMitigacaoResponse
        {
            AgendamentoId = 1,
            Titulo = "Redução Frota",
            MetaReducaoTco2e = 50m,
            TotalEmitidoEmpresa = 150m,
            PercentualImpacto = 33.3333m,
            DataFimPrevista = DateTime.UtcNow.AddMonths(6),
            Status = "Pendente"
        };

        _indicadoresServiceMock
            .Setup(s => s.CalcularImpactoAsync(1))
            .ReturnsAsync(impactoResponse);

        // Act
        var result = await _controller.ImpactoMitigacao(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value.Should().BeOfType<ImpactoMitigacaoResponse>().Subject;
        response.AgendamentoId.Should().Be(1);
        response.PercentualImpacto.Should().Be(33.3333m);
    }
}