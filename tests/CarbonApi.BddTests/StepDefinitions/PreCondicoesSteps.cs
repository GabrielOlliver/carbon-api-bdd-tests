using System.Net;
using System.Text.Json;
using CarbonApi.BddTests.Support;
using FluentAssertions;
using Reqnroll;

namespace CarbonApi.BddTests.StepDefinitions;

/// Steps "Dado" que preparam dados reais via API (sem acesso direto ao banco).
[Binding]
public class PreCondicoesSteps
{
    private readonly ApiContext _api;

    public PreCondicoesSteps(ApiContext api) => _api = api;

    private long EmpresaId => long.Parse(_api.Variaveis["empresaId"]);

    private void GarantirStatus(HttpStatusCode esperado, string acao) =>
        _api.Response!.StatusCode.Should().Be(esperado,
            "a pre-condicao '{0}' precisa funcionar. Corpo: {1}", acao, _api.ResponseBody);

    [Given("que existe uma empresa cadastrada")]
    public async Task DadoQueExisteUmaEmpresaCadastrada()
    {
        var cnpj = ApiContext.GerarCnpj();
        await _api.EnviarAsync(HttpMethod.Post, "/api/v1/empresas-sustentaveis", JsonSerializer.Serialize(new
        {
            nome = $"Empresa BDD {_api.Variaveis["sufixo"]}",
            cnpj,
            setor = "Tecnologia",
            pais = "Brasil"
        }));
        GarantirStatus(HttpStatusCode.Created, "cadastrar empresa");

        _api.Variaveis["empresaId"] = _api.Json["id"]!.ToJsonString();
        _api.Variaveis["empresaCnpj"] = cnpj;
    }

    [Given("que existe uma emissão registrada para essa empresa")]
    public async Task DadoQueExisteUmaEmissaoRegistrada()
    {
        await _api.EnviarAsync(HttpMethod.Post, "/api/v1/relatorio-emissao", JsonSerializer.Serialize(new
        {
            empresaId = EmpresaId,
            quantidadeTco2e = 200.0m,
            escopo = 1,
            descricao = "Emissao de pre-condicao BDD",
            dataEmissao = _api.Variaveis["ontem"]
        }));
        GarantirStatus(HttpStatusCode.Created, "registrar emissao");
        _api.Variaveis["emissaoId"] = _api.Json["id"]!.ToJsonString();
    }

    [Given("que existe um agendamento de redução cadastrado para essa empresa")]
    public async Task DadoQueExisteUmAgendamentoCadastrado()
    {
        await _api.EnviarAsync(HttpMethod.Post, "/api/v1/agendamento-reducao-carbono", JsonSerializer.Serialize(new
        {
            empresaId = EmpresaId,
            titulo = "Troca de frota diesel por eletrica",
            descricao = "Agendamento de pre-condicao BDD",
            metaReducaoTco2e = 50.0m,
            dataInicio = _api.Variaveis["hoje"],
            dataFimPrevista = _api.Variaveis["daqui90dias"]
        }));
        GarantirStatus(HttpStatusCode.Created, "criar agendamento");
        _api.Variaveis["agendamentoId"] = _api.Json["id"]!.ToJsonString();
    }

    [Given("que esse agendamento foi cancelado")]
    public async Task DadoQueEsseAgendamentoFoiCancelado()
    {
        await _api.EnviarAsync(HttpMethod.Delete, "/api/v1/agendamento-reducao-carbono/{agendamentoId}");
        GarantirStatus(HttpStatusCode.NoContent, "cancelar agendamento");
    }
}
