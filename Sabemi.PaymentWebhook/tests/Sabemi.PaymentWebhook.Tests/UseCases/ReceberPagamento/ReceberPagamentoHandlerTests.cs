using Sabemi.PaymentWebhook.Application.UseCases.ReceberPagamento;
using Sabemi.PaymentWebhook.Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sabemi.PaymentWebhook.Application;

namespace Sabemi.PaymentWebhook.Tests.UseCases.ReceberPagamento;

public class ReceberPagamentoHandlerTests
{
    [Fact]
    public async Task Deve_Criar_Pagamento_Atraves_Do_Command()
    {
        // Arrange
        var command = new ReceberPagamentoCommand(
            "TX-001",
            "CTR-001",
            150.50m,
            new DateTime(2026, 8, 14),
            "Sucesso", 
            "{}");

        var services = new ServiceCollection();

        services.AddLogging();
        services.AddApplication();

        var serviceProvider = services.BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        // Act
        var pagamento = await mediator.Send(command);

        // Assert
        Assert.NotNull(pagamento);
        Assert.Equal("TX-001", pagamento.IdTransacao);
        Assert.Equal("CTR-001", pagamento.IdContrato);
        Assert.Equal(150.50m, pagamento.Valor);
        Assert.Equal(PagamentoStatus.Sucesso, pagamento.Status);
    }

    [Fact]
    public async Task Deve_Rejeitar_Status_Invalido()
    {
        // Arrange
        var command = new ReceberPagamentoCommand(
            "TX-001",
            "CTR-001",
            150.50m,
            new DateTime(2026, 8, 14),
            "StatusInexistente",
            "{}");

        var services = new ServiceCollection();

        services.AddLogging();
        services.AddApplication();

        var serviceProvider = services.BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await mediator.Send(command));
    }
}