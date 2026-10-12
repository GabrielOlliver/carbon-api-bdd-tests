using Xunit;
using CarbonApi.Controllers;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarbonApi.Tests.Controllers;

public class EmpresaControllerTests
{
    private readonly Mock<IEmpresaService> _empresaServiceMock;
    private readonly EmpresaController _controller;

    public EmpresaControllerTests()
    {
        _empresaServiceMock = new Mock<IEmpresaService>();
        _controller = new EmpresaController(_empresaServiceMock.Object);
    }

    [Fact]
    public async Task Listar_DeveRetornar200_QuandoExistemEmpresas()
    {
        // Arrange
        var pagedResponse = new PagedResponse<EmpresaResponse>
        {
            Data = new List<EmpresaResponse>
            {
                new() { Id = 1, Nome = "EcoTech Brasil", Cnpj = "12.345.678/0001-99",
                        Setor = "Tecnologia", Pais = "Brasil", Ativa = true }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 1
        };

        _empresaServiceMock
            .Setup(s => s.ListarAsync(1, 10))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.Listar(1, 10);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value.Should().BeOfType<PagedResponse<EmpresaResponse>>().Subject;
        response.TotalRecords.Should().Be(1);
        response.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task BuscarPorId_DeveRetornar200_QuandoEmpresaExiste()
    {
        // Arrange
        var empresaResponse = new EmpresaResponse
        {
            Id = 1,
            Nome = "EcoTech Brasil",
            Cnpj = "12.345.678/0001-99",
            Setor = "Tecnologia",
            Pais = "Brasil",
            Ativa = true
        };

        _empresaServiceMock
            .Setup(s => s.BuscarPorIdAsync(1))
            .ReturnsAsync(empresaResponse);

        // Act
        var result = await _controller.BuscarPorId(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value.Should().BeOfType<EmpresaResponse>().Subject;
        response.Id.Should().Be(1);
        response.Nome.Should().Be("EcoTech Brasil");
    }
}