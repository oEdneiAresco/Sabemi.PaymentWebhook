using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sabemi.PaymentWebhook.Application.UseCases.ConsultarPagamentos;

namespace Sabemi.PaymentWebhook.Api.Controllers;

[ApiController]
[Route("pagamentos")]
public sealed class PagamentosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagamentosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] string? status,
        [FromQuery] string? idContrato,
        CancellationToken cancellationToken)
    {
        var pagamentos = await _mediator.Send(
            new ConsultarPagamentosQuery(
                status,
                idContrato),
            cancellationToken);

        return Ok(pagamentos);
    }
}