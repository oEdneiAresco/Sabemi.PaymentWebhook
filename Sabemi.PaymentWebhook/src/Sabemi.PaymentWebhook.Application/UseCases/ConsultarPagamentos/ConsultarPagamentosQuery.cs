using MediatR;

namespace Sabemi.PaymentWebhook.Application.UseCases.ConsultarPagamentos;

public sealed record ConsultarPagamentosQuery(
    string? Status,
    string? IdContrato)
    : IRequest<IReadOnlyList<PagamentoDto>>;