using CarbonApi.Exceptions;
using CarbonApi.Models;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services;

public class CompensacaoCarbonoService : ICompensacaoCarbonoService
{
    private readonly ICompensacaoCarbonoRepository _repository;
    private readonly IEmpresaRepository _empresaRepository;

    public CompensacaoCarbonoService(
        ICompensacaoCarbonoRepository repository,
        IEmpresaRepository empresaRepository)
    {
        _repository = repository;
        _empresaRepository = empresaRepository;
    }

    public async Task<PagedResponse<CompensacaoCarbonoResponse>> ListarAsync(int page, int pageSize)
    {
        var (data, total) = await _repository.ListarAsync(page, pageSize);
        return new PagedResponse<CompensacaoCarbonoResponse>
        {
            Data = data.Select(MapToResponse),
            PageNumber = page,
            PageSize = pageSize,
            TotalRecords = total
        };
    }

    public async Task<CompensacaoCarbonoResponse> CriarAsync(CompensacaoCarbonoRequest request)
    {
        var empresa = await _empresaRepository.BuscarPorIdAsync(request.EmpresaId)
            ?? throw new NotFoundException($"Empresa não encontrada com id: {request.EmpresaId}");

        if (request.QuantidadeTco2e <= 0)
            throw new BusinessException("Quantidade de tCO2e deve ser maior que zero");

        var compensacao = new CompensacaoCarbono
        {
            EmpresaId = request.EmpresaId,
            Tipo = request.Tipo,
            QuantidadeTco2e = request.QuantidadeTco2e,
            Descricao = request.Descricao,
            CertificadoUrl = request.CertificadoUrl,
            DataCompensacao = request.DataCompensacao,
            DataRegistro = DateTime.UtcNow
        };

        var criada = await _repository.CriarAsync(compensacao);
        criada.Empresa = empresa;
        return MapToResponse(criada);
    }

    private static CompensacaoCarbonoResponse MapToResponse(CompensacaoCarbono c) => new()
    {
        Id = c.Id,
        EmpresaId = c.EmpresaId,
        EmpresaNome = c.Empresa?.Nome ?? string.Empty,
        Tipo = c.Tipo,
        QuantidadeTco2e = c.QuantidadeTco2e,
        Descricao = c.Descricao,
        CertificadoUrl = c.CertificadoUrl,
        DataCompensacao = c.DataCompensacao,
        DataRegistro = c.DataRegistro
    };
}