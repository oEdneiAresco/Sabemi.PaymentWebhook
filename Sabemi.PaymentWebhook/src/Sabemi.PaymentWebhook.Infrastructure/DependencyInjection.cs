using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sabemi.PaymentWebhook.Application.Interfaces;
using Sabemi.PaymentWebhook.Infrastructure.Persistence;
using Sabemi.PaymentWebhook.Infrastructure.Persistence.Repositories;
using Sabemi.PaymentWebhook.Infrastructure.Processing;

namespace Sabemi.PaymentWebhook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<PaymentWebhookDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IPagamentoRepository, PagamentoRepository>();

        services.AddScoped<IPagamentoEventoRepository, PagamentoEventoRepository>();

        services.AddScoped<IStatusContratoRepository, StatusContratoRepository>();

        services.AddSingleton<IProcessamentoPagamentoQueue, ProcessamentoPagamentoQueue>();


        return services;
    }
}