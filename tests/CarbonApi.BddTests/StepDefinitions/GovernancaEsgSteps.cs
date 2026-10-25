using CarbonApi.BddTests.Support;
using FluentAssertions;
using Reqnroll;

namespace CarbonApi.BddTests.StepDefinitions;

/// Regras de negocio ESG (pilar Governanca): garantem que os indicadores
/// ambientais publicados pela API sao matematicamente consistentes.
[Binding]
public class GovernancaEsgSteps
{
    private readonly ApiContext _api;

    public GovernancaEsgSteps(ApiContext api) => _api = api;

    private decimal Num(string campo) => _api.Campo(campo)!.GetValue<decimal>();

    [Then("o saldo líquido deve ser igual ao total emitido menos o total compensado")]
    public void EntaoOSaldoLiquidoDeveSerConsistente() =>
        Num("saldoLiquidoTco2e").Should().Be(Num("totalTco2eEmitido") - Num("totalTco2eCompensado"));

    [Then("o percentual de impacto deve corresponder à meta dividida pelo total emitido")]
    public void EntaoOPercentualDeImpactoDeveSerConsistente()
    {
        var total = Num("totalEmitidoEmpresa");
        var esperado = total > 0 ? Math.Round(Num("metaReducaoTco2e") / total * 100, 4) : 0;
        Num("percentualImpacto").Should().BeApproximately(esperado, 0.001m);
    }
}
