using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services.Interfaces;

public interface IAgendamentoReducaoService
{
    Task<PagedResponse<AgendamentoReducaoResponse>> ListarAsync(int page, int pageSize);
    Task<AgendamentoReducaoResponse> BuscarPorIdAsync(long id);
    Task<AgendamentoReducaoResponse> CriarAsync(AgendamentoReducaoRequest request);
    Task<AgendamentoReducaoResponse> AtualizarAsync(long id, AgendamentoReducaoUpdateRequest request);
    Task CancelarAsync(long id);
}