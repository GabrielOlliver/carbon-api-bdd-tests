using CarbonApi.Models.Enums;

namespace CarbonApi.Models;

public class AgendamentoReducao
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal MetaReducaoTco2e { get; set; }
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
    public DateTime DataInicio { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    public Empresa Empresa { get; set; } = null!;
}