using MediatR;
using Sabemi.PaymentWebhook.Domain.Entities;

namespace Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

public sealed record ReceberPagamentoCommand(
    string IdTransacao,
    string IdContrato,
    decimal Valor,
    DateTime? DataPagamento,
    string Status) : IRequest<Pagamento>;