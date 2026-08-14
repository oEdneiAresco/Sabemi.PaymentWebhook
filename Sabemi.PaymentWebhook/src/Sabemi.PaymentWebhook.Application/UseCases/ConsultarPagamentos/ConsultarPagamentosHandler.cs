using MediatR;
using Sabemi.PaymentWebhook.Application.Interfaces;

namespace Sabemi.PaymentWebhook.Application.UseCases.ConsultarPagamentos;

public sealed class ConsultarPagamentosHandler
    : IRequestHandler<
        ConsultarPagamentosQuery,
        IReadOnlyList<PagamentoDto>>
{
    private readonly IPagamentoRepository _pagamentoRepository;

    public ConsultarPagamentosHandler(
        IPagamentoRepository pagamentoRepository)
    {
        _pagamentoRepository = pagamentoRepository;
    }

    public async Task<IReadOnlyList<PagamentoDto>> Handle(
        ConsultarPagamentosQuery request,
        CancellationToken cancellationToken)
    {
        var pagamentos = await _pagamentoRepository.ListarAsync(
            request.Status,
            request.IdContrato,
            cancellationToken);

        return pagamentos
            .Select(p => new PagamentoDto(
                p.IdTransacao,
                p.IdContrato,
                p.Valor,
                p.DataPagamento,
                p.Status.ToString()))
            .ToList();
    }
}