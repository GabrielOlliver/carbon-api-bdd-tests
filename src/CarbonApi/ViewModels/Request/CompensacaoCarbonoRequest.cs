using CarbonApi.Models.Enums;

namespace CarbonApi.ViewModels.Request;

public class CompensacaoCarbonoRequest
{
    public long EmpresaId { get; set; }
    public TipoCompensacao Tipo { get; set; }
    public decimal QuantidadeTco2e { get; set; }
    public string? Descricao { get; set; }
    public string? CertificadoUrl { get; set; }
    public DateTime DataCompensacao { get; set; }
}