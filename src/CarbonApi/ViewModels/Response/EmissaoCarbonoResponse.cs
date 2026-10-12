using CarbonApi.Models.Enums;

namespace CarbonApi.ViewModels.Response;

public class EmissaoCarbonoResponse
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public decimal QuantidadeTco2e { get; set; }
    public EscopoEmissao Escopo { get; set; }
    public string? Descricao { get; set; }
    public DateTime DataEmissao { get; set; }
    public DateTime DataRegistro { get; set; }
}