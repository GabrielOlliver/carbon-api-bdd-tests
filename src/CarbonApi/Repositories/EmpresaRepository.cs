using CarbonApi.Data;
using CarbonApi.Models;
using CarbonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarbonApi.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly ApplicationDbContext _context;

    public EmpresaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Empresa> Data, long Total)> ListarAsync(int page, int pageSize)
    {
        var query = _context.Empresas.AsNoTracking().OrderBy(e => e.Nome);
        var total = await query.LongCountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, total);
    }

    public async Task<Empresa?> BuscarPorIdAsync(long id)
        => await _context.Empresas.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Empresa?> BuscarPorCnpjAsync(string cnpj)
        => await _context.Empresas.AsNoTracking().FirstOrDefaultAsync(e => e.Cnpj == cnpj);

    public async Task<bool> ExistePorCnpjAsync(string cnpj)
        => await _context.Empresas.AnyAsync(e => e.Cnpj == cnpj);

    public async Task<Empresa> CriarAsync(Empresa empresa)
    {
        _context.Empresas.Add(empresa);
        await _context.SaveChangesAsync();
        return empresa;
    }

    public async Task<Empresa> AtualizarAsync(Empresa empresa)
    {
        _context.Empresas.Update(empresa);
        await _context.SaveChangesAsync();
        return empresa;
    }
}