using CarbonApi.Models;

namespace CarbonApi.Repositories.Interfaces;

public interface ICompensacaoCarbonoRepository
{
    Task<(IEnumerable<CompensacaoCarbono> Data, long Total)> ListarAsync(int page, int pageSize);
    Task<CompensacaoCarbono> CriarAsync(CompensacaoCarbono compensacao);
    Task<decimal> SomarPorEmpresaAsync(long empresaId);
}