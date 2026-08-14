using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sabemi.PaymentWebhook.Api.Contracts;
using Sabemi.PaymentWebhook.Application.UseCases.ConsultarPagamentos;
using Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;
using System.Text.Json;

namespace Sabemi.PaymentWebhook.Api.Controllers;

[ApiController]
[Route("webhooks")]
public sealed class WebhooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebhooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("pagamento")]
    public async Task<IActionResult> ReceberPagamento(
        [FromBody] ReceberPagamentoRequest request)
    {
        var payload = JsonSerializer.Serialize(request);

        var command = new ReceberPagamentoCommand(
            request.IdTransacao,
            request.IdContrato,
            request.Valor,
            request.DataPagamento,
            request.Status,
            payload);

        await _mediator.Send(command);

        return Accepted();
    }

    [HttpGet("pagamento")]
    public async Task<IActionResult> ConsultarPagamentos(
    [FromQuery] string? status,
    [FromQuery] string? idContrato,
    CancellationToken cancellationToken)
    {
        var query = new ConsultarPagamentosQuery(
            status,
            idContrato);

        var pagamentos = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(pagamentos);
    }
}