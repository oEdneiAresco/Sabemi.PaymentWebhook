import { useEffect, useState } from "react";
import { buscarPagamentos } from "./api/pagamentosApi";

function App() {
  const [pagamentos, setPagamentos] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState(null);

  useEffect(() => {
    async function carregarPagamentos() {
      try {
        const dados = await buscarPagamentos();
        setPagamentos(dados);
      } catch (error) {
        setErro(error.message);
      } finally {
        setCarregando(false);
      }
    }

    carregarPagamentos();
  }, []);

  return (
    <div>
      <header>
        <h1>Sabemi Payment Webhook</h1>
        <p>Monitoramento de pagamentos</p>
      </header>

      <main>
        <section>
          <h2>Pagamentos</h2>

          {carregando && <p>Carregando pagamentos...</p>}

          {erro && <p>Erro: {erro}</p>}

          {!carregando && !erro && (
            <p>
              {pagamentos.length} pagamento(s) encontrado(s).
            </p>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;