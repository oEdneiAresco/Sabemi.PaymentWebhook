const API_URL = "https://localhost:7197";

export async function buscarPagamentos() {
  const response = await fetch(
    `${API_URL}/webhooks/pagamento`,
    {
      method: "GET",
      headers: {
        "X-Api-Key": "sabemi-webhook-2026"
      }
    }
  );

  if (!response.ok) {
    throw new Error(
      `Erro ao consultar pagamentos: ${response.status}`
    );
  }

  return response.json();
}