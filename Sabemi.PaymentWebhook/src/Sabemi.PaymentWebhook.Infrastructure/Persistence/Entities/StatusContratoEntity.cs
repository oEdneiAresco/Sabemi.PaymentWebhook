namespace Sabemi.PaymentWebhook.Infrastructure.Persistence.Entities;

public class StatusContratoEntity
{
    public Guid Id { get; set; }

    public string IdContrato { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime AtualizadoEm { get; set; }
}