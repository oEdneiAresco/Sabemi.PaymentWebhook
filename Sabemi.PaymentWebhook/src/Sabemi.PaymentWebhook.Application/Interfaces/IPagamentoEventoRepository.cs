namespace Sabemi.PaymentWebhook.Application.Interfaces;

public interface IPagamentoEventoRepository
{
    Task<Guid> AdicionarAsync(
        string idTransacao,
        string payload,
        DateTime recebidoEm,
        bool processado,
        string? erro,
        CancellationToken cancellationToken);

    Task MarcarComoProcessadoAsync(
    Guid id,
    string? erro,
    CancellationToken cancellationToken);
}