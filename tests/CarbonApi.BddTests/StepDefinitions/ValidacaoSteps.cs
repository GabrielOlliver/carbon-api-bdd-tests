using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using CarbonApi.BddTests.Support;
using FluentAssertions;
using NJsonSchema;
using Reqnroll;

namespace CarbonApi.BddTests.StepDefinitions;

/// Steps "Entao": validam status code, corpo JSON e contrato (JSON Schema).
[Binding]
public class ValidacaoSteps
{
    private readonly ApiContext _api;

    public ValidacaoSteps(ApiContext api) => _api = api;

    [Then("o status da resposta deve ser {int}")]
    public void EntaoOStatusDeveSer(int esperado) =>
        ((int)_api.Response!.StatusCode).Should().Be(esperado, "corpo da resposta: {0}", _api.ResponseBody);

    [Then("a resposta deve respeitar o contrato {string}")]
    public async Task EntaoARespostaDeveRespeitarOContrato(string arquivo)
    {
        var caminho = Path.Combine(AppContext.BaseDirectory, "Schemas", arquivo);
        File.Exists(caminho).Should().BeTrue("o schema {0} deve existir na pasta Schemas", arquivo);

        var schema = await JsonSchema.FromFileAsync(caminho);
        var erros = schema.Validate(_api.ResponseBody);

        erros.Should().BeEmpty("a resposta deve seguir o contrato {0}. Violacoes: {1}. Corpo: {2}",
            arquivo, string.Join("; ", erros.Select(e => $"{e.Path} -> {e.Kind}")), _api.ResponseBody);
    }

    [Then("o campo {string} da resposta deve ser {string}")]
    public void EntaoOCampoDeveSer(string campo, string esperado)
    {
        var no = _api.Campo(campo);
        var valorEsperado = _api.Resolver(esperado);

        // Numeros sao comparados como numeros: o Oracle devolve 150.5 como 150.5000
        if (no is JsonValue valor && valor.GetValueKind() == JsonValueKind.Number &&
            decimal.TryParse(valorEsperado, NumberStyles.Number, CultureInfo.InvariantCulture, out var numero))
        {
            valor.GetValue<decimal>().Should().Be(numero,
                "o campo '{0}' deve ter o valor esperado. Corpo: {1}", campo, _api.ResponseBody);
            return;
        }

        ApiContext.ComoTexto(no).Should().Be(valorEsperado,
            "o campo '{0}' deve ter o valor esperado. Corpo: {1}", campo, _api.ResponseBody);
    }

    [Then("o campo {string} da resposta deve estar preenchido")]
    public void EntaoOCampoDeveEstarPreenchido(string campo) =>
        ApiContext.ComoTexto(_api.Campo(campo)).Should().NotBeNullOrWhiteSpace().And.NotBe("null");

    [Then("o campo {string} da resposta deve ser maior ou igual a {int}")]
    public void EntaoOCampoDeveSerMaiorOuIgual(string campo, int minimo) =>
        _api.Campo(campo)!.GetValue<decimal>().Should().BeGreaterThanOrEqualTo(minimo);

    [Then("a lista {string} da resposta não deve estar vazia")]
    public void EntaoAListaNaoDeveEstarVazia(string campo) =>
        _api.Campo(campo).Should().BeOfType<JsonArray>().Which.Count.Should().BeGreaterThan(0);

    [Then("todos os itens da lista {string} devem ter o campo {string} igual a {string}")]
    public void EntaoTodosOsItensDevemTerOCampo(string lista, string campo, string esperado)
    {
        var itens = _api.Campo(lista).Should().BeOfType<JsonArray>().Subject;
        itens.Should().NotBeEmpty();
        foreach (var item in itens)
            ApiContext.ComoTexto(item![campo]).Should().Be(_api.Resolver(esperado));
    }

    [Then("a mensagem de erro deve conter {string}")]
    public void EntaoAMensagemDeErroDeveConter(string trecho) =>
        ApiContext.ComoTexto(_api.Campo("mensagem")).Should().Contain(_api.Resolver(trecho));

    [Then("o corpo da resposta deve ser {string}")]
    public void EntaoOCorpoDaRespostaDeveSer(string esperado) =>
        _api.ResponseBody.Should().Be(esperado);
}
