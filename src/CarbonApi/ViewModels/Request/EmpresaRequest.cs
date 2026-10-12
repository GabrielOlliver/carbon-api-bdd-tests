namespace CarbonApi.ViewModels.Request;

public class EmpresaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Setor { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
}