using MediatR;
using Sabemi.PaymentWebhook.Application.Interfaces;
using Sabemi.PaymentWebhook.Domain.Entities;
using Sabemi.PaymentWebhook.Domain.Enums;

namespace Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

public sealed class ReceberPagamentoHandler
    : IRequestHandler<ReceberPagamentoCommand, Pagamento>
{
    private readonly IPagamentoEventoRepository _pagamentoEventoRepository;
    private readonly IProcessamentoPagamentoQueue _queue;

    public ReceberPagamentoHandler(
        IPagamentoEventoRepository pagamentoEventoRepository,
        IProcessamentoPagamentoQueue queue)
    {
        _pagamentoEventoRepository = pagamentoEventoRepository;
        _queue = queue;
    }

    public async Task<Pagamento> Handle(
        ReceberPagamentoCommand command,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<PagamentoStatus>(
                command.Status,
                true,
                out var status))
        {
            throw new ArgumentException(
                "O status do pagamento é inválido.",
                nameof(command.Status));
        }

        var dataPagamento = command.DataPagamento
            ?? throw new ArgumentException(
                "Data de pagamento é obrigatória.");

        var eventoId =
            await _pagamentoEventoRepository.AdicionarAsync(
                command.IdTransacao,
                command.Payload,
                DateTime.UtcNow,
                false,
                null,
                cancellationToken);

        var processamentoCommand = new ProcessarPagamentoCommand(
            eventoId,
            command.IdTransacao,
            command.IdContrato,
            command.Valor,
            dataPagamento,
            command.Status);

        await _queue.EnfileirarAsync(
            processamentoCommand,
            cancellationToken);

        return Pagamento.Create(
            command.IdTransacao,
            command.IdContrato,
            command.Valor,
            dataPagamento,
            status);
    }
}