using CarbonApi.BddTests.Support;
using Reqnroll;

namespace CarbonApi.BddTests.StepDefinitions;

/// Steps "Quando" genericos: executam a chamada HTTP descrita no cenario.
[Binding]
public class RequisicaoSteps
{
    private readonly ApiContext _api;

    public RequisicaoSteps(ApiContext api) => _api = api;

    [When("eu envio uma requisição GET para {string}")]
    public Task QuandoEuEnvioGet(string caminho) => _api.EnviarAsync(HttpMethod.Get, caminho);

    [When("eu envio uma requisição DELETE para {string}")]
    public Task QuandoEuEnvioDelete(string caminho) => _api.EnviarAsync(HttpMethod.Delete, caminho);

    [When("eu envio uma requisição POST para {string} com o corpo:")]
    public Task QuandoEuEnvioPost(string caminho, string corpo) => _api.EnviarAsync(HttpMethod.Post, caminho, corpo);

    [When("eu envio uma requisição PUT para {string} com o corpo:")]
    public Task QuandoEuEnvioPut(string caminho, string corpo) => _api.EnviarAsync(HttpMethod.Put, caminho, corpo);
}
