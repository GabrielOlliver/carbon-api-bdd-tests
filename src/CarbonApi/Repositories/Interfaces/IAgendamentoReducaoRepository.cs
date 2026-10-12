using CarbonApi.Models;

namespace CarbonApi.Repositories.Interfaces;

public interface IAgendamentoReducaoRepository
{
    Task<(IEnumerable<AgendamentoReducao> Data, long Total)> ListarAsync(int page, int pageSize);
    Task<AgendamentoReducao?> BuscarPorIdAsync(long id);
    Task<AgendamentoReducao> CriarAsync(AgendamentoReducao agendamento);
    Task<AgendamentoReducao> AtualizarAsync(AgendamentoReducao agendamento);
    Task<long> ContarConcluidosAsync();
}