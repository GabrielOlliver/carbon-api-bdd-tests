using System.Net;
using System.Text.Json;
using CarbonApi.BddTests.Support;
using FluentAssertions;
using Reqnroll;

namespace CarbonApi.BddTests.StepDefinitions;

[Binding]
public class AutenticacaoSteps
{
    private const string LoginUrl = "/api/v1/auth/login";
    private readonly ApiContext _api;

    public AutenticacaoSteps(ApiContext api) => _api = api;

    [Given("que estou autenticado como administrador")]
    public async Task DadoQueEstouAutenticadoComoAdministrador()
    {
        await _api.EnviarAsync(HttpMethod.Post, LoginUrl,
            JsonSerializer.Serialize(new { email = "admin@esg.com", senha = "admin123" }));

        _api.Response!.StatusCode.Should().Be(HttpStatusCode.OK,
            "o login do administrador e pre-condicao do cenario. Corpo: {0}", _api.ResponseBody);

        _api.Token = _api.Json["token"]!.GetValue<string>();
    }

    [Given("que não estou autenticado")]
    public void DadoQueNaoEstouAutenticado() => _api.Token = null;

    [Given("que estou autenticado com um token inválido")]
    public void DadoQueEstouAutenticadoComUmTokenInvalido() =>
        _api.Token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJmYWtlIn0.assinaturaInvalida";

    [When("eu faço login com o email {string} e a senha {string}")]
    public Task QuandoEuFacoLogin(string email, string senha) =>
        _api.EnviarAsync(HttpMethod.Post, LoginUrl, JsonSerializer.Serialize(new { email, senha }));
}
