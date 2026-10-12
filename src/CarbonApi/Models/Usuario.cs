namespace CarbonApi.Models;

public class Usuario
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Role { get; set; } = "EMPRESA";
    public long? EmpresaId { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public Empresa? Empresa { get; set; }
}