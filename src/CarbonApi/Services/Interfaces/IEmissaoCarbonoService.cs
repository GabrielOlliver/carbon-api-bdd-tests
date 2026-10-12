using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services.Interfaces;

public interface IEmissaoCarbonoService
{
    Task<PagedResponse<EmissaoCarbonoResponse>> ListarAsync(int page, int pageSize);
    Task<PagedResponse<EmissaoCarbonoResponse>> ListarPorEmpresaAsync(long empresaId, int page, int pageSize);
    Task<EmissaoCarbonoResponse> RegistrarAsync(EmissaoCarbonoRequest request);
}