using CarbonApi.Models.Enums;

namespace CarbonApi.ViewModels.Request;

public class AgendamentoReducaoUpdateRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal MetaReducaoTco2e { get; set; }
    public StatusAgendamento Status { get; set; }
    public DateTime DataFimPrevista { get; set; }
}