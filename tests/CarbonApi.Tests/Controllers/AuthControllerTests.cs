using Xunit;
using CarbonApi.Controllers;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarbonApi.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Login_DeveRetornar200_QuandoCredenciaisValidas()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "admin@esg.com",
            Senha = "admin123"
        };

        var loginResponse = new LoginResponse
        {
            Token = "fake-jwt-token",
            Tipo = "Bearer",
            Email = "admin@esg.com",
            Role = "ADMIN"
        };

        _authServiceMock
            .Setup(s => s.AutenticarAsync(request))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value.Should().BeOfType<LoginResponse>().Subject;
        response.Email.Should().Be("admin@esg.com");
        response.Role.Should().Be("ADMIN");
    }
}