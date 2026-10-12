namespace CarbonApi.ViewModels.Request;

public class AgendamentoReducaoRequest
{
    public long EmpresaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal MetaReducaoTco2e { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFimPrevista { get; set; }
}