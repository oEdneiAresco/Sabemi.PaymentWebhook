namespace Sabemi.PaymentWebhook.Infrastructure.Persistence.Entities;

public class PagamentoEventoEntity
{
    public Guid Id { get; set; }

    public string IdTransacao { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public DateTime RecebidoEm { get; set; }

    public bool Processado { get; set; }

    public string? Erro { get; set; }
}