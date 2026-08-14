using Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

namespace Sabemi.PaymentWebhook.Application.Interfaces;

public interface IProcessamentoPagamentoQueue
{
    ValueTask EnfileirarAsync(
        ProcessarPagamentoCommand command,
        CancellationToken cancellationToken);

    IAsyncEnumerable<ProcessarPagamentoCommand> LerAsync(
        CancellationToken cancellationToken);
}