using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services.Interfaces;

public interface ICompensacaoCarbonoService
{
    Task<PagedResponse<CompensacaoCarbonoResponse>> ListarAsync(int page, int pageSize);
    Task<CompensacaoCarbonoResponse> CriarAsync(CompensacaoCarbonoRequest request);
}