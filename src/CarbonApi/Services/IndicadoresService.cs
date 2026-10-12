using CarbonApi.Exceptions;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Response;
using CarbonApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CarbonApi.Services;

public class IndicadoresService : IIndicadoresService
{
    private readonly ApplicationDbContext _context;
    private readonly IAgendamentoReducaoRepository _agendamentoRepository;
    private readonly IEmissaoCarbonoRepository _emissaoRepository;

    public IndicadoresService(
        ApplicationDbContext context,
        IAgendamentoReducaoRepository agendamentoRepository,
        IEmissaoCarbonoRepository emissaoRepository)
    {
        _context = context;
        _agendamentoRepository = agendamentoRepository;
        _emissaoRepository = emissaoRepository;
    }

    public async Task<IndicadoresResponse> CalcularIndicadoresAsync()
    {
        var totalEmpresas = await _context.Empresas.LongCountAsync();
        var totalEmissoes = await _context.EmissoesCarbono.LongCountAsync();
        var totalAgendamentos = await _context.AgendamentosReducao.LongCountAsync();
        var agendamentosConcluidos = await _agendamentoRepository.ContarConcluidosAsync();

        var totalEmitido = await _context.EmissoesCarbono
            .SumAsync(e => (decimal?)e.QuantidadeTco2e) ?? 0;

        var totalCompensado = await _context.CompensacoesCarbono
            .SumAsync(c => (decimal?)c.QuantidadeTco2e) ?? 0;

        return new IndicadoresResponse
        {
            TotalEmpresas = totalEmpresas,
            TotalEmissoes = totalEmissoes,
            TotalTco2eEmitido = totalEmitido,
            TotalAgendamentos = totalAgendamentos,
            AgendamentosConcluidos = agendamentosConcluidos,
            TotalTco2eCompensado = totalCompensado,
            SaldoLiquidoTco2e = totalEmitido - totalCompensado
        };
    }

    public async Task<ImpactoMitigacaoResponse> CalcularImpactoAsync(long agendamentoId)
    {
        var agendamento = await _agendamentoRepository.BuscarPorIdAsync(agendamentoId)
            ?? throw new NotFoundException($"Agendamento não encontrado com id: {agendamentoId}");

        var totalEmitido = await _emissaoRepository.SomarPorEmpresaAsync(agendamento.EmpresaId);

        var percentual = totalEmitido > 0
            ? Math.Round(agendamento.MetaReducaoTco2e / totalEmitido * 100, 4)
            : 0;

        return new ImpactoMitigacaoResponse
        {
            AgendamentoId = agendamento.Id,
            Titulo = agendamento.Titulo,
            MetaReducaoTco2e = agendamento.MetaReducaoTco2e,
            TotalEmitidoEmpresa = totalEmitido,
            PercentualImpacto = percentual,
            DataFimPrevista = agendamento.DataFimPrevista,
            Status = agendamento.Status.ToString()
        };
    }
}