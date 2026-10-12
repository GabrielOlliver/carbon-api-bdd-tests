using CarbonApi.Models.Enums;

namespace CarbonApi.Models;

public class EmissaoCarbono
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public decimal QuantidadeTco2e { get; set; }
    public EscopoEmissao Escopo { get; set; }
    public string? Descricao { get; set; }
    public DateTime DataEmissao { get; set; }
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;

    public Empresa Empresa { get; set; } = null!;
}