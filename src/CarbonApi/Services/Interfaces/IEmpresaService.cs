using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services.Interfaces;

public interface IEmpresaService
{
    Task<PagedResponse<EmpresaResponse>> ListarAsync(int page, int pageSize);
    Task<EmpresaResponse> BuscarPorIdAsync(long id);
    Task<EmpresaResponse> CriarAsync(EmpresaRequest request);
}