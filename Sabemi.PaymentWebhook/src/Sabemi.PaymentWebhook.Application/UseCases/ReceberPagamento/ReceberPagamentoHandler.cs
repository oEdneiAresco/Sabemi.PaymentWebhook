using MediatR;
using Sabemi.PaymentWebhook.Domain.Entities;
using Sabemi.PaymentWebhook.Domain.Enums;

namespace Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

public sealed class ReceberPagamentoHandler
    : IRequestHandler<ReceberPagamentoCommand, Pagamento>
{
    public Task<Pagamento> Handle(
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

        var pagamento = Pagamento.Create(
            command.IdTransacao,
            command.IdContrato,
            command.Valor,
            command.DataPagamento,
            status);

        return Task.FromResult(pagamento);
    }
}