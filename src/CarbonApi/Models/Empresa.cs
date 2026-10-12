namespace CarbonApi.Models;

public class Empresa
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Setor { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public bool Ativa { get; set; } = true;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    public ICollection<EmissaoCarbono> Emissoes { get; set; } = new List<EmissaoCarbono>();
    public ICollection<AgendamentoReducao> Agendamentos { get; set; } = new List<AgendamentoReducao>();
    public ICollection<CompensacaoCarbono> Compensacoes { get; set; } = new List<CompensacaoCarbono>();
}