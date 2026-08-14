using Microsoft.EntityFrameworkCore;
using Sabemi.PaymentWebhook.Application.Interfaces;
using Sabemi.PaymentWebhook.Infrastructure.Persistence;
using Sabemi.PaymentWebhook.Infrastructure.Persistence.Entities;

namespace Sabemi.PaymentWebhook.Infrastructure.Persistence.Repositories;

public sealed class StatusContratoRepository
    : IStatusContratoRepository
{
    private readonly PaymentWebhookDbContext _context;

    public StatusContratoRepository(
        PaymentWebhookDbContext context)
    {
        _context = context;
    }

    public async Task AtualizarAsync(
        string idContrato,
        string status,
        DateTime atualizadoEm,
        CancellationToken cancellationToken)
    {
        var entity = await _context.StatusContratos
            .SingleOrDefaultAsync(
                x => x.IdContrato == idContrato,
                cancellationToken);

        if (entity is null)
        {
            entity = new StatusContratoEntity
            {
                Id = Guid.NewGuid(),
                IdContrato = idContrato,
                Status = status,
                AtualizadoEm = atualizadoEm
            };

            await _context.StatusContratos.AddAsync(
                entity,
                cancellationToken);
        }
        else
        {
            entity.Status = status;
            entity.AtualizadoEm = atualizadoEm;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}