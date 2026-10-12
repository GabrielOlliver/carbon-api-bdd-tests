using CarbonApi.Data;
using CarbonApi.Models;
using CarbonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarbonApi.Repositories;

public class EmissaoCarbonoRepository : IEmissaoCarbonoRepository
{
    private readonly ApplicationDbContext _context;

    public EmissaoCarbonoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<EmissaoCarbono> Data, long Total)> ListarAsync(int page, int pageSize)
    {
        var query = _context.EmissoesCarbono
            .AsNoTracking()
            .Include(e => e.Empresa)
            .OrderByDescending(e => e.DataEmissao);
        var total = await query.LongCountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, total);
    }

    public async Task<(IEnumerable<EmissaoCarbono> Data, long Total)> ListarPorEmpresaAsync(
        long empresaId, int page, int pageSize)
    {
        var query = _context.EmissoesCarbono
            .AsNoTracking()
            .Include(e => e.Empresa)
            .Where(e => e.EmpresaId == empresaId)
            .OrderByDescending(e => e.DataEmissao);
        var total = await query.LongCountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, total);
    }

    public async Task<EmissaoCarbono?> BuscarPorIdAsync(long id)
        => await _context.EmissoesCarbono
            .AsNoTracking()
            .Include(e => e.Empresa)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<EmissaoCarbono> CriarAsync(EmissaoCarbono emissao)
    {
        _context.EmissoesCarbono.Add(emissao);
        await _context.SaveChangesAsync();
        return emissao;
    }

    public async Task<decimal> SomarPorEmpresaAsync(long empresaId)
        => await _context.EmissoesCarbono
            .Where(e => e.EmpresaId == empresaId)
            .SumAsync(e => e.QuantidadeTco2e);
}