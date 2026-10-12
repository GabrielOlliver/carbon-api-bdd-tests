using CarbonApi.Data;
using CarbonApi.Models;
using CarbonApi.Models.Enums;
using CarbonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarbonApi.Repositories;

public class AgendamentoReducaoRepository : IAgendamentoReducaoRepository
{
    private readonly ApplicationDbContext _context;

    public AgendamentoReducaoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<AgendamentoReducao> Data, long Total)> ListarAsync(
        int page, int pageSize)
    {
        var query = _context.AgendamentosReducao
            .AsNoTracking()
            .Include(a => a.Empresa)
            .OrderByDescending(a => a.DataCadastro);
        var total = await query.LongCountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, total);
    }

    public async Task<AgendamentoReducao?> BuscarPorIdAsync(long id)
        => await _context.AgendamentosReducao
            .AsNoTracking()
            .Include(a => a.Empresa)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<AgendamentoReducao> CriarAsync(AgendamentoReducao agendamento)
    {
        _context.AgendamentosReducao.Add(agendamento);
        await _context.SaveChangesAsync();
        return agendamento;
    }

    public async Task<AgendamentoReducao> AtualizarAsync(AgendamentoReducao agendamento)
    {
        _context.AgendamentosReducao.Update(agendamento);
        await _context.SaveChangesAsync();
        return agendamento;
    }

    public async Task<long> ContarConcluidosAsync()
        => await _context.AgendamentosReducao
            .LongCountAsync(a => a.Status == StatusAgendamento.Concluido);
}