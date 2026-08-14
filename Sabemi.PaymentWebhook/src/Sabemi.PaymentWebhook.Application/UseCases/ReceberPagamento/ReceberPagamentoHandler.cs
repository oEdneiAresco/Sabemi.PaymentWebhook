using MediatR;
using Sabemi.PaymentWebhook.Domain.Entities;
using Sabemi.PaymentWebhook.Domain.Enums;
using Sabemi.PaymentWebhook.Application.Interfaces;

namespace Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;

public sealed class ReceberPagamentoHandler
    : IRequestHandler<ReceberPagamentoCommand, Pagamento>
{
    private readonly IPagamentoRepository _pagamentoRepository;

    public ReceberPagamentoHandler(
        IPagamentoRepository pagamentoRepository)
    {
        _pagamentoRepository = pagamentoRepository;
    }

    public async Task<Pagamento> Handle(
        ReceberPagamentoCommand command,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<PagamentoStatus>(
                command.Status,
                true,
                out var status))
        {
            throw new ArgumentException(
                "O status do pagamento é inválido.",
                nameof(command.Status));
        }

        var dataPagamento = command.DataPagamento
            ?? throw new ArgumentException("Data de pagamento é obrigatória.");

        var pagamentoExistente =
            await _pagamentoRepository.ObterPorIdTransacaoAsync(
                command.IdTransacao,
                cancellationToken);

        if (pagamentoExistente is not null)
        {
            return pagamentoExistente;
        }

        var pagamento = Pagamento.Create(
            command.IdTransacao,
            command.IdContrato,
            command.Valor,
            dataPagamento,
            status);

        await _pagamentoRepository.AdicionarAsync(
            pagamento,
            cancellationToken);

        return pagamento;
    }

}