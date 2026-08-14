using Sabemi.PaymentWebhook.Domain.Entities;
using Sabemi.PaymentWebhook.Domain.Enums;

namespace Sabemi.PaymentWebhook.Tests;

public class PagamentoTests
{
    [Fact]
    public void Deve_Criar_Pagamento_Quando_Dados_Sao_Validos()
    {
        // Arrange
        var dataPagamento = new DateTime(2026, 8, 14);

        // Act
        var pagamento = Pagamento.Create(
            "TX-001",
            "CTR-001",
            150.50m,
            dataPagamento,
            PagamentoStatus.Sucesso);

        // Assert
        Assert.NotNull(pagamento);
        Assert.Equal("TX-001", pagamento.IdTransacao);
        Assert.Equal("CTR-001", pagamento.IdContrato);
        Assert.Equal(150.50m, pagamento.Valor);
        Assert.Equal(dataPagamento, pagamento.DataPagamento);
        Assert.Equal(PagamentoStatus.Sucesso, pagamento.Status);
    }

    [Fact]
    public void Deve_Rejeitar_Pagamento_Quando_IdTransacao_Estiver_Vazio()
    {
        // Arrange
        var dataPagamento = new DateTime(2026, 8, 14);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Pagamento.Create(
                "",
                "CTR-001",
                150.50m,
                dataPagamento,
                PagamentoStatus.Sucesso));
    }

    [Fact]
    public void Deve_Rejeitar_Pagamento_Quando_IdContrato_Estiver_Vazio()
    {
        var dataPagamento = new DateTime(2026, 8, 14);

        Assert.Throws<ArgumentException>(() =>
            Pagamento.Create(
                "TX-001",
                "",
                150.50m,
                dataPagamento,
                PagamentoStatus.Sucesso));
    }

    [Fact]
    public void Deve_Rejeitar_Pagamento_Quando_Valor_For_Zero()
    {
        var dataPagamento = new DateTime(2026, 8, 14);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Pagamento.Create(
                "TX-001",
                "CTR-001",
                0m,
                dataPagamento,
                PagamentoStatus.Sucesso));
    }

    [Fact]
    public void Deve_Rejeitar_Pagamento_Quando_Valor_For_Negativo()
    {
        var dataPagamento = new DateTime(2026, 8, 14);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Pagamento.Create(
                "TX-001",
                "CTR-001",
                -10m,
                dataPagamento,
                PagamentoStatus.Sucesso));
    }

    [Fact]
    public void Deve_Rejeitar_Pagamento_Quando_Data_For_Invalida()
    {
        Assert.Throws<ArgumentException>(() =>
            Pagamento.Create(
                "TX-001",
                "CTR-001",
                150.50m,
                default,
                PagamentoStatus.Sucesso));
    }

    [Fact]
    public void Deve_Rejeitar_Pagamento_Quando_Status_For_Invalido()
    {
        var dataPagamento = new DateTime(2026, 8, 14);

        var statusInvalido = (PagamentoStatus)99;

        Assert.Throws<ArgumentException>(() =>
            Pagamento.Create(
                "TX-001",
                "CTR-001",
                150.50m,
                dataPagamento,
                statusInvalido));
    }
}