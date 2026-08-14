using Microsoft.EntityFrameworkCore;
using Sabemi.PaymentWebhook.Application.Interfaces;
using Sabemi.PaymentWebhook.Domain.Entities;
using Sabemi.PaymentWebhook.Domain.Enums;
using Sabemi.PaymentWebhook.Infrastructure.Persistence;
using Sabemi.PaymentWebhook.Infrastructure.Persistence.Entities;

namespace Sabemi.PaymentWebhook.Infrastructure.Persistence.Repositories;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly PaymentWebhookDbContext _context;

    public PagamentoRepository(PaymentWebhookDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(
        Pagamento pagamento,
        CancellationToken cancellationToken)
    {
        var entity = new PagamentoEntity
        {
            IdTransacao = pagamento.IdTransacao,
            IdContrato = pagamento.IdContrato,
            Valor = pagamento.Valor,
            DataPagamento = pagamento.DataPagamento,
            Status = pagamento.Status.ToString()
        };

        await _context.Pagamentos.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Pagamento?> ObterPorIdTransacaoAsync(
        string idTransacao,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Pagamentos
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.IdTransacao == idTransacao,
                cancellationToken);

        if (entity is null)
            return null;

        return Pagamento.Create(
            entity.IdTransacao,
            entity.IdContrato,
            entity.Valor,
            entity.DataPagamento,
            Enum.Parse<PagamentoStatus>(entity.Status));
    }
}