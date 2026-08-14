const API_URL = "https://localhost:7197";

export async function buscarPagamentos({
  status = "",
  idContrato = ""
} = {}) {
  const parametros = new URLSearchParams();

  if (status) {
    parametros.append("status", status);
  }

  if (idContrato) {
    parametros.append("idContrato", idContrato);
  }

  const queryString = parametros.toString();

  const url = queryString
    ? `${API_URL}/webhooks/pagamento?${queryString}`
    : `${API_URL}/webhooks/pagamento`;

  const response = await fetch(url, {
    method: "GET",
    headers: {
      "X-Api-Key": "sabemi-webhook-2026"
    }
  });

  if (!response.ok) {
    throw new Error(
      `Erro ao consultar pagamentos: ${response.status}`
    );
  }

  return response.json();
}