namespace Sabemi.PaymentWebhook.Application.UseCases.ConsultarPagamentos;

public sealed record PagamentoDto(
    string IdTransacao,
    string IdContrato,
    decimal Valor,
    DateTime DataPagamento,
    string Status);