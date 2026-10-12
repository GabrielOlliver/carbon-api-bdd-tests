using CarbonApi.Exceptions;
using CarbonApi.Models;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _repository;

    public EmpresaService(IEmpresaRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<EmpresaResponse>> ListarAsync(int page, int pageSize)
    {
        var (data, total) = await _repository.ListarAsync(page, pageSize);
        return new PagedResponse<EmpresaResponse>
        {
            Data = data.Select(MapToResponse),
            PageNumber = page,
            PageSize = pageSize,
            TotalRecords = total
        };
    }

    public async Task<EmpresaResponse> BuscarPorIdAsync(long id)
    {
        var empresa = await _repository.BuscarPorIdAsync(id)
            ?? throw new NotFoundException($"Empresa não encontrada com id: {id}");
        return MapToResponse(empresa);
    }

    public async Task<EmpresaResponse> CriarAsync(EmpresaRequest request)
    {
        if (await _repository.ExistePorCnpjAsync(request.Cnpj))
            throw new ConflictException($"Empresa com CNPJ {request.Cnpj} já cadastrada");

        var empresa = new Empresa
        {
            Nome = request.Nome,
            Cnpj = request.Cnpj,
            Setor = request.Setor,
            Pais = request.Pais,
            Ativa = true,
            DataCadastro = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        var criada = await _repository.CriarAsync(empresa);
        return MapToResponse(criada);
    }

    private static EmpresaResponse MapToResponse(Empresa e) => new()
    {
        Id = e.Id,
        Nome = e.Nome,
        Cnpj = e.Cnpj,
        Setor = e.Setor,
        Pais = e.Pais,
        Ativa = e.Ativa,
        DataCadastro = e.DataCadastro
    };
}