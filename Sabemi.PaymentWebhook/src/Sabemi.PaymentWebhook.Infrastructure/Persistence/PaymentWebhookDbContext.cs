using Microsoft.EntityFrameworkCore;
using Sabemi.PaymentWebhook.Infrastructure.Persistence.Entities;

namespace Sabemi.PaymentWebhook.Infrastructure.Persistence;

public class PaymentWebhookDbContext : DbContext
{
    public PaymentWebhookDbContext(
        DbContextOptions<PaymentWebhookDbContext> options)
        : base(options)
    {
    }

    public DbSet<PagamentoEntity> Pagamentos => Set<PagamentoEntity>();

    public DbSet<PagamentoEventoEntity> PagamentoEventos =>
        Set<PagamentoEventoEntity>();

    public DbSet<StatusContratoEntity> StatusContratos =>
        Set<StatusContratoEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PagamentoEntity>()
            .HasIndex(x => x.IdTransacao)
            .IsUnique();

        modelBuilder.Entity<StatusContratoEntity>()
            .HasIndex(x => x.IdContrato)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}