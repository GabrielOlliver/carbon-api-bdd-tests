using CarbonApi.Models.Enums;

namespace CarbonApi.ViewModels.Request;

public class EmissaoCarbonoRequest
{
    public long EmpresaId { get; set; }
    public decimal QuantidadeTco2e { get; set; }
    public EscopoEmissao Escopo { get; set; }
    public string? Descricao { get; set; }
    public DateTime DataEmissao { get; set; }
}