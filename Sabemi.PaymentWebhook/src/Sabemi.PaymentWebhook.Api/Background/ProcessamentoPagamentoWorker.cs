using MediatR;
using Sabemi.PaymentWebhook.Application.Interfaces;

namespace Sabemi.PaymentWebhook.Api.Background;

public sealed class ProcessamentoPagamentoWorker : BackgroundService
{
    private readonly IProcessamentoPagamentoQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProcessamentoPagamentoWorker> _logger;

    public ProcessamentoPagamentoWorker(
        IProcessamentoPagamentoQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<ProcessamentoPagamentoWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        await foreach (var command in _queue.LerAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var mediator = scope.ServiceProvider
                    .GetRequiredService<IMediator>();

                await mediator.Send(command, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao processar pagamento em background.");
            }
        }
    }
}