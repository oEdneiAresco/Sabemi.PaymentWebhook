using System.Text.Json.Serialization;

namespace Sabemi.PaymentWebhook.Api.Contracts;

public sealed class ReceberPagamentoRequest
{
    [JsonPropertyName("id_transacao")]
    public string IdTransacao { get; init; } = string.Empty;

    [JsonPropertyName("id_contrato")]
    public string IdContrato { get; init; } = string.Empty;

    [JsonPropertyName("valor")]
    public decimal Valor { get; init; }

    [JsonPropertyName("data_pagamento")]
    public DateTime? DataPagamento { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;
}