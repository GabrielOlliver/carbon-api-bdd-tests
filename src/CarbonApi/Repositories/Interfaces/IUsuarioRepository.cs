using CarbonApi.Models;

namespace CarbonApi.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> BuscarPorEmailAsync(string email);
    Task<bool> ExistePorEmailAsync(string email);
}