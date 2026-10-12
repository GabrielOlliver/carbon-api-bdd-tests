namespace CarbonApi.ViewModels.Response;

public class ImpactoMitigacaoResponse
{
    public long AgendamentoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public decimal MetaReducaoTco2e { get; set; }
    public decimal TotalEmitidoEmpresa { get; set; }
    public decimal PercentualImpacto { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public string Status { get; set; } = string.Empty;
}