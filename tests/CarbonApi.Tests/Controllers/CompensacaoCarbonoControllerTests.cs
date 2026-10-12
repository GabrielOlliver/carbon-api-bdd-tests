using Xunit;
using CarbonApi.Controllers;
using CarbonApi.Models.Enums;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarbonApi.Tests.Controllers;

public class CompensacaoCarbonoControllerTests
{
    private readonly Mock<ICompensacaoCarbonoService> _compensacaoServiceMock;
    private readonly CompensacaoCarbonoController _controller;

    public CompensacaoCarbonoControllerTests()
    {
        _compensacaoServiceMock = new Mock<ICompensacaoCarbonoService>();
        _controller = new CompensacaoCarbonoController(_compensacaoServiceMock.Object);
    }

    [Fact]
    public async Task Listar_DeveRetornar200_QuandoExistemCompensacoes()
    {
        // Arrange
        var pagedResponse = new PagedResponse<CompensacaoCarbonoResponse>
        {
            Data = new List<CompensacaoCarbonoResponse>
            {
                new() { Id = 1, EmpresaId = 1, EmpresaNome = "EcoTech Brasil",
                    Tipo = TipoCompensacao.Reflorestamento,
                    QuantidadeTco2e = 30m,
                    DataCompensacao = DateTime.UtcNow,
                    DataRegistro = DateTime.UtcNow }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 1
        };

        _compensacaoServiceMock
            .Setup(s => s.ListarAsync(1, 10))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.Listar(1, 10);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value
            .Should().BeOfType<PagedResponse<CompensacaoCarbonoResponse>>().Subject;
        response.TotalRecords.Should().Be(1);
    }
}