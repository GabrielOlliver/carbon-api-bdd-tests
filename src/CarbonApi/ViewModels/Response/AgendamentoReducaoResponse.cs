using CarbonApi.Models.Enums;

namespace CarbonApi.ViewModels.Response;

public class AgendamentoReducaoResponse
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal MetaReducaoTco2e { get; set; }
    public StatusAgendamento Status { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
}