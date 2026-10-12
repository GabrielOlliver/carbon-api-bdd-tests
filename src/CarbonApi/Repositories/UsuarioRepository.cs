using CarbonApi.Data;
using CarbonApi.Models;
using CarbonApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarbonApi.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
        => await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> ExistePorEmailAsync(string email)
        => await _context.Usuarios.AnyAsync(u => u.Email == email);
}