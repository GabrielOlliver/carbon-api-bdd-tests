using CarbonApi.Exceptions;
using CarbonApi.Models;
using CarbonApi.Models.Enums;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services;

public class AgendamentoReducaoService : IAgendamentoReducaoService
{
    private readonly IAgendamentoReducaoRepository _repository;
    private readonly IEmpresaRepository _empresaRepository;

    public AgendamentoReducaoService(
        IAgendamentoReducaoRepository repository,
        IEmpresaRepository empresaRepository)
    {
        _repository = repository;
        _empresaRepository = empresaRepository;
    }

    public async Task<PagedResponse<AgendamentoReducaoResponse>> ListarAsync(int page, int pageSize)
    {
        var (data, total) = await _repository.ListarAsync(page, pageSize);
        return new PagedResponse<AgendamentoReducaoResponse>
        {
            Data = data.Select(MapToResponse),
            PageNumber = page,
            PageSize = pageSize,
            TotalRecords = total
        };
    }

    public async Task<AgendamentoReducaoResponse> BuscarPorIdAsync(long id)
    {
        var agendamento = await _repository.BuscarPorIdAsync(id)
            ?? throw new NotFoundException($"Agendamento não encontrado com id: {id}");
        return MapToResponse(agendamento);
    }

    public async Task<AgendamentoReducaoResponse> CriarAsync(AgendamentoReducaoRequest request)
    {
        var empresa = await _empresaRepository.BuscarPorIdAsync(request.EmpresaId)
            ?? throw new NotFoundException($"Empresa não encontrada com id: {request.EmpresaId}");

        if (request.DataFimPrevista <= request.DataInicio)
            throw new BusinessException("Data fim prevista deve ser posterior à data de início");

        if (request.MetaReducaoTco2e <= 0)
            throw new BusinessException("Meta de redução deve ser maior que zero");

        var agendamento = new AgendamentoReducao
        {
            EmpresaId = request.EmpresaId,
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            MetaReducaoTco2e = request.MetaReducaoTco2e,
            Status = StatusAgendamento.Pendente,
            DataInicio = request.DataInicio,
            DataFimPrevista = request.DataFimPrevista,
            DataCadastro = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        var criado = await _repository.CriarAsync(agendamento);
        criado.Empresa = empresa;
        return MapToResponse(criado);
    }

    public async Task<AgendamentoReducaoResponse> AtualizarAsync(
        long id, AgendamentoReducaoUpdateRequest request)
    {
        var agendamento = await _repository.BuscarPorIdAsync(id)
            ?? throw new NotFoundException($"Agendamento não encontrado com id: {id}");

        if (agendamento.Status == StatusAgendamento.Cancelado)
            throw new BusinessException("Não é possível atualizar um agendamento cancelado");

        agendamento.Titulo = request.Titulo;
        agendamento.Descricao = request.Descricao;
        agendamento.MetaReducaoTco2e = request.MetaReducaoTco2e;
        agendamento.Status = request.Status;
        agendamento.DataFimPrevista = request.DataFimPrevista;
        agendamento.DataAtualizacao = DateTime.UtcNow;

        var atualizado = await _repository.AtualizarAsync(agendamento);
        return MapToResponse(atualizado);
    }

    public async Task CancelarAsync(long id)
    {
        var agendamento = await _repository.BuscarPorIdAsync(id)
            ?? throw new NotFoundException($"Agendamento não encontrado com id: {id}");

        if (agendamento.Status == StatusAgendamento.Cancelado)
            throw new BusinessException("Agendamento já está cancelado");

        if (agendamento.Status == StatusAgendamento.Concluido)
            throw new BusinessException("Não é possível cancelar um agendamento concluído");

        agendamento.Status = StatusAgendamento.Cancelado;
        agendamento.DataAtualizacao = DateTime.UtcNow;
        await _repository.AtualizarAsync(agendamento);
    }

    private static AgendamentoReducaoResponse MapToResponse(AgendamentoReducao a) => new()
    {
        Id = a.Id,
        EmpresaId = a.EmpresaId,
        EmpresaNome = a.Empresa?.Nome ?? string.Empty,
        Titulo = a.Titulo,
        Descricao = a.Descricao,
        MetaReducaoTco2e = a.MetaReducaoTco2e,
        Status = a.Status,
        DataInicio = a.DataInicio,
        DataFimPrevista = a.DataFimPrevista,
        DataCadastro = a.DataCadastro,
        DataAtualizacao = a.DataAtualizacao
    };
}