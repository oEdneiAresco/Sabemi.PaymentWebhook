using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sabemi.PaymentWebhook.Application.Interfaces;
using Sabemi.PaymentWebhook.Infrastructure.Persistence;
using Sabemi.PaymentWebhook.Infrastructure.Persistence.Entities;

namespace Sabemi.PaymentWebhook.Infrastructure.Persistence.Repositories;

public class PagamentoEventoRepository : IPagamentoEventoRepository
{
    private readonly PaymentWebhookDbContext _context;

    public PagamentoEventoRepository(PaymentWebhookDbContext context)
    {
        _context = context;
    }

    public async Task<(Guid Id, bool Novo)> AdicionarAsync(
        string idTransacao,
        string payload,
        DateTime recebidoEm,
        bool processado,
        string? erro,
        CancellationToken cancellationToken)
    {
        var entity = new PagamentoEventoEntity
        {
            Id = Guid.NewGuid(),
            IdTransacao = idTransacao,
            Payload = payload,
            RecebidoEm = recebidoEm,
            Processado = processado,
            Erro = erro
        };

        await _context.PagamentoEventos.AddAsync(
            entity,
            cancellationToken);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            return (entity.Id, true);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlException &&
                  (sqlException.Number == 2601 ||
                   sqlException.Number == 2627))
        {
            _context.Entry(entity).State = EntityState.Detached;

            var eventoExistente =
                await _context.PagamentoEventos
                    .SingleAsync(
                        x => x.IdTransacao == idTransacao,
                        cancellationToken);

            return (eventoExistente.Id, false);
        }
    }

    public async Task MarcarComoProcessadoAsync(
        Guid id,
        string? erro,
        CancellationToken cancellationToken)
    {
        var entity = await _context.PagamentoEventos
            .FindAsync(
                new object[] { id },
                cancellationToken);

        if (entity is null)
            throw new InvalidOperationException(
                "Evento de pagamento não encontrado.");

        entity.Processado = true;
        entity.Erro = erro;

        await _context.SaveChangesAsync(cancellationToken);
    }
}