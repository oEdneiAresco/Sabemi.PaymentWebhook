using Sabemi.PaymentWebhook.Domain.Enums;

namespace Sabemi.PaymentWebhook.Domain.Entities;

public class Pagamento
{
    public string IdTransacao { get; private set; }
    public string IdContrato { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataPagamento { get; private set; }
    public PagamentoStatus Status { get; private set; }

    private Pagamento(
        string idTransacao,
        string idContrato,
        decimal valor,
        DateTime dataPagamento,
        PagamentoStatus status)
    {
        IdTransacao = idTransacao;
        IdContrato = idContrato;
        Valor = valor;
        DataPagamento = dataPagamento;
        Status = status;
    }

    public static Pagamento Create(
        string idTransacao,
        string idContrato,
        decimal valor,
        DateTime dataPagamento,
        PagamentoStatus status)
    {
        ValidarIdTransacao(idTransacao);
        ValidarIdContrato(idContrato);
        ValidarValor(valor);
        ValidarDataPagamento(dataPagamento);
        ValidarStatus(status);

        return new Pagamento(
            idTransacao,
            idContrato,
            valor,
            dataPagamento,
            status);
    }

    private static void ValidarIdTransacao(string idTransacao)
    {
        if (string.IsNullOrWhiteSpace(idTransacao))
            throw new ArgumentException(
                "O ID da transação é obrigatório.",
                nameof(idTransacao));
    }

    private static void ValidarIdContrato(string idContrato)
    {
        if (string.IsNullOrWhiteSpace(idContrato))
            throw new ArgumentException(
                "O ID do contrato é obrigatório.",
                nameof(idContrato));
    }

    private static void ValidarValor(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(valor),
                "O valor do pagamento deve ser maior que zero.");
    }

    private static void ValidarDataPagamento(DateTime dataPagamento)
    {
        if (dataPagamento == default)
            throw new ArgumentException(
                "A data de pagamento é obrigatória.",
                nameof(dataPagamento));
    }

    private static void ValidarStatus(PagamentoStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentException(
                "O status do pagamento é inválido.",
                nameof(status));
    }
}