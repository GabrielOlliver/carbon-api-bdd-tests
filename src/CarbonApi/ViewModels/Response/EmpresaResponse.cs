namespace CarbonApi.ViewModels.Response;

public class EmpresaResponse
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Setor { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public bool Ativa { get; set; }
    public DateTime DataCadastro { get; set; }
}