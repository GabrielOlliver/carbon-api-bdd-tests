using CarbonApi.Models;

namespace CarbonApi.Repositories.Interfaces;

public interface IEmpresaRepository
{
    Task<(IEnumerable<Empresa> Data, long Total)> ListarAsync(int page, int pageSize);
    Task<Empresa?> BuscarPorIdAsync(long id);
    Task<Empresa?> BuscarPorCnpjAsync(string cnpj);
    Task<bool> ExistePorCnpjAsync(string cnpj);
    Task<Empresa> CriarAsync(Empresa empresa);
    Task<Empresa> AtualizarAsync(Empresa empresa);
}