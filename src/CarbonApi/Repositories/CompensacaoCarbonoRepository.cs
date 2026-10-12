using CarbonApi.Data;
using CarbonApi.Models;
using CarbonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarbonApi.Repositories;

public class CompensacaoCarbonoRepository : ICompensacaoCarbonoRepository
{
    private readonly ApplicationDbContext _context;

    public CompensacaoCarbonoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<CompensacaoCarbono> Data, long Total)> ListarAsync(
        int page, int pageSize)
    {
        var query = _context.CompensacoesCarbono
            .AsNoTracking()
            .Include(c => c.Empresa)
            .OrderByDescending(c => c.DataCompensacao);
        var total = await query.LongCountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, total);
    }

    public async Task<CompensacaoCarbono> CriarAsync(CompensacaoCarbono compensacao)
    {
        _context.CompensacoesCarbono.Add(compensacao);
        await _context.SaveChangesAsync();
        return compensacao;
    }

    public async Task<decimal> SomarPorEmpresaAsync(long empresaId)
        => await _context.CompensacoesCarbono
            .Where(c => c.EmpresaId == empresaId)
            .SumAsync(c => c.QuantidadeTco2e);
}