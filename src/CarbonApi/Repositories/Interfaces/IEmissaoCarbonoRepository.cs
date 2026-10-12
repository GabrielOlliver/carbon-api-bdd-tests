using CarbonApi.Models;

namespace CarbonApi.Repositories.Interfaces;

public interface IEmissaoCarbonoRepository
{
    Task<(IEnumerable<EmissaoCarbono> Data, long Total)> ListarAsync(int page, int pageSize);
    Task<(IEnumerable<EmissaoCarbono> Data, long Total)> ListarPorEmpresaAsync(long empresaId, int page, int pageSize);
    Task<EmissaoCarbono?> BuscarPorIdAsync(long id);
    Task<EmissaoCarbono> CriarAsync(EmissaoCarbono emissao);
    Task<decimal> SomarPorEmpresaAsync(long empresaId);
}