using Sabemi.PaymentWebhook.Domain.Entities;

namespace Sabemi.PaymentWebhook.Application.Interfaces;

public interface IPagamentoRepository
{
    Task AdicionarAsync(
        Pagamento pagamento,
        CancellationToken cancellationToken);

    Task<Pagamento?> ObterPorIdTransacaoAsync(
        string idTransacao,
        CancellationToken cancellationToken);
}