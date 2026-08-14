using MediatR;

namespace Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

public sealed record ProcessarPagamentoCommand(
    Guid EventoId,
    string IdTransacao,
    string IdContrato,
    decimal Valor,
    DateTime DataPagamento,
    string Status) : IRequest;