using Xunit;
using CarbonApi.Controllers;
using CarbonApi.Models.Enums;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarbonApi.Tests.Controllers;

public class EmissaoCarbonoControllerTests
{
    private readonly Mock<IEmissaoCarbonoService> _emissaoServiceMock;
    private readonly EmissaoCarbonoController _controller;

    public EmissaoCarbonoControllerTests()
    {
        _emissaoServiceMock = new Mock<IEmissaoCarbonoService>();
        _controller = new EmissaoCarbonoController(_emissaoServiceMock.Object);
    }

    [Fact]
    public async Task Listar_DeveRetornar200_QuandoExistemEmissoes()
    {
        // Arrange
        var pagedResponse = new PagedResponse<EmissaoCarbonoResponse>
        {
            Data = new List<EmissaoCarbonoResponse>
            {
                new() { Id = 1, EmpresaId = 1, EmpresaNome = "EcoTech Brasil",
                        QuantidadeTco2e = 150.5m, Escopo = EscopoEmissao.Escopo1,
                        DataEmissao = DateTime.UtcNow, DataRegistro = DateTime.UtcNow }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 1
        };

        _emissaoServiceMock
            .Setup(s => s.ListarAsync(1, 10))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.Listar(1, 10);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value
            .Should().BeOfType<PagedResponse<EmissaoCarbonoResponse>>().Subject;
        response.TotalRecords.Should().Be(1);
    }

    [Fact]
    public async Task HistoricoPorEmpresa_DeveRetornar200_QuandoEmpresaExiste()
    {
        // Arrange
        var pagedResponse = new PagedResponse<EmissaoCarbonoResponse>
        {
            Data = new List<EmissaoCarbonoResponse>(),
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 0
        };

        _emissaoServiceMock
            .Setup(s => s.ListarPorEmpresaAsync(1, 1, 10))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.HistoricoPorEmpresa(1, 1, 10);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }
}