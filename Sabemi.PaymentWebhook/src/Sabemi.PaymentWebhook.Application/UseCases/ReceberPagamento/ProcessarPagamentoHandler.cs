using MediatR;
using Sabemi.PaymentWebhook.Application.Interfaces;
using Sabemi.PaymentWebhook.Domain.Entities;
using Sabemi.PaymentWebhook.Domain.Enums;

namespace Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

public sealed class ProcessarPagamentoHandler
    : IRequestHandler<ProcessarPagamentoCommand>
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IPagamentoEventoRepository _pagamentoEventoRepository;

    public ProcessarPagamentoHandler(
        IPagamentoRepository pagamentoRepository,
        IPagamentoEventoRepository pagamentoEventoRepository)
    {
        _pagamentoRepository = pagamentoRepository;
        _pagamentoEventoRepository = pagamentoEventoRepository;
    }

    public async Task Handle(
        ProcessarPagamentoCommand command,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<PagamentoStatus>(
                command.Status,
                true,
                out var status))
        {
            await _pagamentoEventoRepository.MarcarComoProcessadoAsync(
                command.EventoId,
                "O status do pagamento é inválido.",
                cancellationToken);

            return;
        }

        try
        {
            var pagamentoExistente =
                await _pagamentoRepository.ObterPorIdTransacaoAsync(
                    command.IdTransacao,
                    cancellationToken);

            if (pagamentoExistente is null)
            {
                var pagamento = Pagamento.Create(
                    command.IdTransacao,
                    command.IdContrato,
                    command.Valor,
                    command.DataPagamento,
                    status);

                await _pagamentoRepository.AdicionarAsync(
                    pagamento,
                    cancellationToken);
            }

            await _pagamentoEventoRepository.MarcarComoProcessadoAsync(
                command.EventoId,
                null,
                cancellationToken);
        }
        catch (Exception ex)
        {
            await _pagamentoEventoRepository.MarcarComoProcessadoAsync(
                command.EventoId,
                ex.Message,
                cancellationToken);

            throw;
        }
    }
}