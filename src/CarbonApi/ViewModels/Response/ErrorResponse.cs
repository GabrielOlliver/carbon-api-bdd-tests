namespace CarbonApi.ViewModels.Response;

public class ErrorResponse
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int Status { get; set; }
    public string Erro { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string Caminho { get; set; } = string.Empty;
    public List<FieldError>? ErrosCampos { get; set; }
}

public class FieldError
{
    public string Campo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}