using CarbonApi.Models.Enums;

namespace CarbonApi.ViewModels.Response;

public class CompensacaoCarbonoResponse
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public TipoCompensacao Tipo { get; set; }
    public decimal QuantidadeTco2e { get; set; }
    public string? Descricao { get; set; }
    public string? CertificadoUrl { get; set; }
    public DateTime DataCompensacao { get; set; }
    public DateTime DataRegistro { get; set; }
}