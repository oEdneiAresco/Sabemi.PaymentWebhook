import { useEffect, useState } from "react";
import { buscarPagamentos } from "./api/pagamentosApi";

function App() {
  const [pagamentos, setPagamentos] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState(null);

  const [idContrato, setIdContrato] = useState("");
  const [status, setStatus] = useState("");

  async function carregarPagamentos(filtros = {}) {
    try {
      setCarregando(true);
      setErro(null);

      const dados = await buscarPagamentos(filtros);

      setPagamentos(dados);
    } catch (error) {
      setErro(error.message);
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregarPagamentos();
  }, []);

  function filtrar() {
    carregarPagamentos({
      status,
      idContrato
    });
  }

  function limparFiltros() {
    setStatus("");
    setIdContrato("");

    carregarPagamentos();
  }

  function formatarValor(valor) {
    return valor.toLocaleString("pt-BR", {
      style: "currency",
      currency: "BRL"
    });
  }

  function formatarData(data) {
    return new Date(data).toLocaleString("pt-BR");
  }

  return (
    <div>
      <header>
        <h1>Sabemi Payment Webhook</h1>
        <p>Monitoramento de pagamentos</p>
      </header>

      <main>
        <section>
          <h2>Pagamentos</h2>

          <div>
            <input
              type="text"
              placeholder="ID do contrato"
              value={idContrato}
              onChange={(event) => setIdContrato(event.target.value)}
            />

            <select
              value={status}
              onChange={(event) => setStatus(event.target.value)}
            >
              <option value="">Todos os status</option>
              <option value="Sucesso">Sucesso</option>
              <option value="Falha">Falha</option>
            </select>

            <button onClick={filtrar}>
              Filtrar
            </button>

            <button onClick={limparFiltros}>
              Limpar
            </button>
          </div>

          {carregando && <p>Carregando pagamentos...</p>}

          {erro && <p>Erro: {erro}</p>}

          {!carregando && !erro && pagamentos.length === 0 && (
            <p>Nenhum pagamento encontrado.</p>
          )}

          {!carregando && !erro && pagamentos.length > 0 && (
            <>
              <p>
                {pagamentos.length} pagamento(s) encontrado(s).
              </p>

              <table>
                <thead>
                  <tr>
                    <th>Transação</th>
                    <th>Contrato</th>
                    <th>Valor</th>
                    <th>Data</th>
                    <th>Status</th>
                  </tr>
                </thead>

                <tbody>
                  {pagamentos.map((pagamento) => (
                    <tr key={pagamento.idTransacao}>
                      <td>{pagamento.idTransacao}</td>
                      <td>{pagamento.idContrato}</td>
                      <td>{formatarValor(pagamento.valor)}</td>
                      <td>{formatarData(pagamento.dataPagamento)}</td>
                      <td>{pagamento.status}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;