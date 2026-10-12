using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services.Interfaces;

public interface IIndicadoresService
{
    Task<IndicadoresResponse> CalcularIndicadoresAsync();
    Task<ImpactoMitigacaoResponse> CalcularImpactoAsync(long agendamentoId);
}