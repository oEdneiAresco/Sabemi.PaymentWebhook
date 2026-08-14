using System.Threading.Channels;
using Sabemi.PaymentWebhook.Application.Interfaces;
using Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

namespace Sabemi.PaymentWebhook.Infrastructure.Processing;

public sealed class ProcessamentoPagamentoQueue
    : IProcessamentoPagamentoQueue
{
    private readonly Channel<ProcessarPagamentoCommand> _queue;

    public ProcessamentoPagamentoQueue()
    {
        _queue = Channel.CreateUnbounded<ProcessarPagamentoCommand>();
    }

    public ValueTask EnfileirarAsync(
        ProcessarPagamentoCommand command,
        CancellationToken cancellationToken)
    {
        return _queue.Writer.WriteAsync(
            command,
            cancellationToken);
    }

    public IAsyncEnumerable<ProcessarPagamentoCommand> LerAsync(
        CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAllAsync(cancellationToken);
    }
}