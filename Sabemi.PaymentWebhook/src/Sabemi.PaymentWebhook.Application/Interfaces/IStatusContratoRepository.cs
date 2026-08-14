namespace Sabemi.PaymentWebhook.Application.Interfaces;

public interface IStatusContratoRepository
{
    Task AtualizarAsync(
        string idContrato,
        string status,
        DateTime atualizadoEm,
        CancellationToken cancellationToken);
}