using CarbonApi.Exceptions;
using CarbonApi.Models;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services;

public class EmissaoCarbonoService : IEmissaoCarbonoService
{
    private readonly IEmissaoCarbonoRepository _repository;
    private readonly IEmpresaRepository _empresaRepository;

    public EmissaoCarbonoService(
        IEmissaoCarbonoRepository repository,
        IEmpresaRepository empresaRepository)
    {
        _repository = repository;
        _empresaRepository = empresaRepository;
    }

    public async Task<PagedResponse<EmissaoCarbonoResponse>> ListarAsync(int page, int pageSize)
    {
        var (data, total) = await _repository.ListarAsync(page, pageSize);
        return new PagedResponse<EmissaoCarbonoResponse>
        {
            Data = data.Select(MapToResponse),
            PageNumber = page,
            PageSize = pageSize,
            TotalRecords = total
        };
    }

    public async Task<PagedResponse<EmissaoCarbonoResponse>> ListarPorEmpresaAsync(
        long empresaId, int page, int pageSize)
    {
        var empresa = await _empresaRepository.BuscarPorIdAsync(empresaId)
            ?? throw new NotFoundException($"Empresa não encontrada com id: {empresaId}");

        var (data, total) = await _repository.ListarPorEmpresaAsync(empresaId, page, pageSize);
        return new PagedResponse<EmissaoCarbonoResponse>
        {
            Data = data.Select(MapToResponse),
            PageNumber = page,
            PageSize = pageSize,
            TotalRecords = total
        };
    }

    public async Task<EmissaoCarbonoResponse> RegistrarAsync(EmissaoCarbonoRequest request)
    {
        var empresa = await _empresaRepository.BuscarPorIdAsync(request.EmpresaId)
            ?? throw new NotFoundException($"Empresa não encontrada com id: {request.EmpresaId}");

        if (request.QuantidadeTco2e <= 0)
            throw new BusinessException("Quantidade de tCO2e deve ser maior que zero");

        if (request.DataEmissao > DateTime.UtcNow)
            throw new BusinessException("Data da emissão não pode ser futura");

        var emissao = new EmissaoCarbono
        {
            EmpresaId = request.EmpresaId,
            QuantidadeTco2e = request.QuantidadeTco2e,
            Escopo = request.Escopo,
            Descricao = request.Descricao,
            DataEmissao = request.DataEmissao,
            DataRegistro = DateTime.UtcNow
        };

        var criada = await _repository.CriarAsync(emissao);
        criada.Empresa = empresa;
        return MapToResponse(criada);
    }

    private static EmissaoCarbonoResponse MapToResponse(EmissaoCarbono e) => new()
    {
        Id = e.Id,
        EmpresaId = e.EmpresaId,
        EmpresaNome = e.Empresa?.Nome ?? string.Empty,
        QuantidadeTco2e = e.QuantidadeTco2e,
        Escopo = e.Escopo,
        Descricao = e.Descricao,
        DataEmissao = e.DataEmissao,
        DataRegistro = e.DataRegistro
    };
}