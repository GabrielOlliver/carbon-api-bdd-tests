using CarbonApi.Models.Enums;

namespace CarbonApi.Models;

public class CompensacaoCarbono
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public TipoCompensacao Tipo { get; set; }
    public decimal QuantidadeTco2e { get; set; }
    public string? Descricao { get; set; }
    public string? CertificadoUrl { get; set; }
    public DateTime DataCompensacao { get; set; }
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;

    public Empresa Empresa { get; set; } = null!;
}