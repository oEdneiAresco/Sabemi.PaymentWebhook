# Sabemi Payment Webhook

Aplicação desenvolvida como avaliação técnica para processamento de pagamentos recebidos via webhook, com processamento assíncrono, controle de idempotência, atualização do status do contrato e dashboard administrativo para consulta dos pagamentos.

## Visão geral

O sistema recebe notificações de pagamento através de um endpoint HTTP, registra o evento recebido, coloca o processamento em uma fila e realiza o processamento em background.

Após o processamento, o pagamento é persistido e o status do contrato é atualizado.

A aplicação também disponibiliza um dashboard web simples para consulta e filtragem dos pagamentos processados.

## Arquitetura

O projeto utiliza uma separação em camadas, buscando manter as responsabilidades bem definidas:

```text
┌─────────────────────┐
│     Frontend        │
│       React         │
└──────────┬──────────┘
           │ HTTP
           ▼
┌─────────────────────┐
│         API         │
│    ASP.NET Core     │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│    Application      │
│   MediatR / CQRS    │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│   Infrastructure    │
│ EF Core / Repos.    │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│      SQL Server     │
└─────────────────────┘

Processamento do webhook:

Webhook
   ↓
ReceberPagamento
   ↓
PagamentoEvento
   ↓
Fila
   ↓
Background Worker
   ↓
Pagamento
   ↓
StatusContrato
```

## Fluxo do pagamento

1. O sistema recebe um pagamento através do webhook.
2. O evento recebido é persistido em `PagamentoEventos`.
3. O evento é colocado na fila de processamento.
4. Um `BackgroundService` processa o pagamento de forma assíncrona.
5. O pagamento é persistido em `Pagamentos`.
6. O status do contrato é atualizado em `StatusContratos`.
7. O evento é marcado como processado.
8. O pagamento fica disponível para consulta através da API e do dashboard.

## Idempotência

A aplicação trata a idempotência utilizando `IdTransacao` como identificador único do pagamento.

Além da validação na aplicação, existe uma restrição de unicidade no banco de dados:

```text
PagamentoEventos.IdTransacao → UNIQUE
Pagamentos.IdTransacao       → UNIQUE
```

Essa abordagem evita a criação de registros duplicados quando o mesmo webhook é recebido mais de uma vez e também protege a integridade dos dados em cenários de concorrência.

## Tecnologias

### Backend

* .NET 10
* ASP.NET Core
* C#
* MediatR
* Entity Framework Core
* SQL Server
* BackgroundService
* CQRS
* Repository Pattern

### Frontend

* React
* Vite
* JavaScript
* ESLint

### Infraestrutura

* SQL Server
* Entity Framework Core Migrations

## Estrutura do projeto

```text
Sabemi.PaymentWebhook
│
├── src
│   ├── Sabemi.PaymentWebhook.Domain
│   ├── Sabemi.PaymentWebhook.Application
│   ├── Sabemi.PaymentWebhook.Infrastructure
│   └── Sabemi.PaymentWebhook.Api
│
├── tests
│   └── Sabemi.PaymentWebhook.Tests
│
├── frontend
│   └── React + Vite
│
└── README.md
```

## Requisitos

Para executar o projeto localmente:

* .NET 10 SDK
* SQL Server
* Node.js 20.19+ ou versão compatível com o Vite utilizado
* npm

## Configuração do banco

Configure a connection string no `appsettings.json` ou através de configuração local do ambiente.

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SabemiPaymentWebhook;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Depois execute as migrations:

```powershell
Update-Database
```

Ou através da CLI:

```bash
dotnet ef database update
```

## Executando a API

Execute o projeto:

```text
Sabemi.PaymentWebhook.Api
```

A API ficará disponível, conforme a configuração local, em:

```text
https://localhost:7197
```

## Webhook

Endpoint para recebimento de pagamentos:

```http
POST /webhooks/pagamento
```

Header:

```http
X-Api-Key: sabemi-webhook-2026
```

Exemplo de requisição:

```json
{
  "id_transacao": "TRX-001-TESTE",
  "id_contrato": "CTR-001",
  "valor": 150.75,
  "data_pagamento": "2026-08-14T10:30:00",
  "status": "Sucesso"
}
```

Resposta esperada:

```text
202 Accepted
```

O processamento do pagamento ocorre de forma assíncrona após o recebimento do webhook.

## Consulta de pagamentos

Endpoint:

```http
GET /webhooks/pagamento
```

Header:

```http
X-Api-Key: sabemi-webhook-2026
```

A consulta permite filtrar os pagamentos por:

* status;
* contrato.

Exemplo:

```http
GET /webhooks/pagamento?status=Sucesso
```

## Frontend

Entre na pasta do frontend:

```powershell
cd frontend
```

Instale as dependências:

```powershell
npm install
```

Execute:

```powershell
npm run dev
```

O dashboard ficará disponível em:

```text
http://localhost:5173
```

O dashboard permite consultar e filtrar os pagamentos processados pela API.

## Processamento assíncrono

O processamento utiliza um worker em background para desacoplar o recebimento do webhook do processamento efetivo do pagamento.

Dessa forma, o endpoint pode responder rapidamente ao sistema externo com `202 Accepted`, enquanto o processamento continua de forma assíncrona.

## Tratamento de eventos

Os eventos recebidos são armazenados em `PagamentoEventos`, permitindo acompanhar o processamento e registrar eventuais erros.

Cada evento possui informações como:

* identificador;
* transação;
* payload original;
* data de recebimento;
* status de processamento;
* erro, quando existente.

## Testes

Os testes automatizados estão no projeto:

```text
tests/Sabemi.PaymentWebhook.Tests
```

Para executar:

```bash
dotnet test
```

## Decisões técnicas

### Processamento assíncrono

O recebimento do webhook foi separado do processamento do pagamento para evitar que operações de persistência e atualização de contrato mantenham o sistema externo aguardando desnecessariamente.

### Idempotência

A unicidade de `IdTransacao` é garantida também no banco de dados, evitando depender exclusivamente de verificações realizadas pela aplicação.

### Separação de responsabilidades

A aplicação utiliza camadas distintas para domínio, casos de uso, infraestrutura e API, facilitando manutenção, testes e evolução do sistema.

### Dashboard

O frontend foi mantido propositalmente simples, priorizando a consulta e o monitoramento dos pagamentos em vez de adicionar funcionalidades visuais sem impacto no objetivo da aplicação.

## Status do projeto

Projeto funcional contendo:

* recebimento de webhook;
* validação do pagamento;
* persistência de eventos;
* idempotência;
* processamento assíncrono;
* persistência de pagamentos;
* atualização de status de contrato;
* consulta e filtros;
* API protegida por API Key;
* dashboard React;
* migrations do banco;
* testes automatizados.

---

**Sabemi Payment Webhook — Avaliação Técnica**
