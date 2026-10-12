namespace CarbonApi.ViewModels.Response;

public class IndicadoresResponse
{
    public long TotalEmpresas { get; set; }
    public long TotalEmissoes { get; set; }
    public decimal TotalTco2eEmitido { get; set; }
    public long TotalAgendamentos { get; set; }
    public long AgendamentosConcluidos { get; set; }
    public decimal TotalTco2eCompensado { get; set; }
    public decimal SaldoLiquidoTco2e { get; set; }
}