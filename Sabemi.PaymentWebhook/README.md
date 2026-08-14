\# Sabemi Payment Webhook



Sistema de recebimento, processamento e monitoramento de pagamentos via webhook.



O projeto foi desenvolvido com foco em \*\*Clean Architecture\*\*, processamento assíncrono, \*\*idempotência\*\*, separação de responsabilidades e testes automatizados.



\## Visão geral



A aplicação recebe eventos de pagamento através de um webhook, registra o evento recebido, evita processamento duplicado e encaminha pagamentos novos para processamento assíncrono.



Também possui um frontend em React para consulta e filtragem dos pagamentos processados.



\## Arquitetura



O projeto está organizado em camadas:



```text

src/

├── Sabemi.PaymentWebhook.Api

├── Sabemi.PaymentWebhook.Application

├── Sabemi.PaymentWebhook.Domain

└── Sabemi.PaymentWebhook.Infrastructure



tests/

└── Sabemi.PaymentWebhook.Tests



frontend/

└── React + Vite

```



\### Principais responsabilidades



\*\*Domain\*\*



\* Entidades e regras de domínio

\* Enums e objetos relacionados ao pagamento



\*\*Application\*\*



\* Casos de uso

\* Commands e Queries

\* Handlers com MediatR

\* Interfaces para persistência e processamento



\*\*Infrastructure\*\*



\* Entity Framework Core

\* SQL Server

\* Implementações dos repositórios

\* Persistência dos eventos



\*\*API\*\*



\* Endpoint de webhook

\* Endpoint de consulta de pagamentos

\* API Key

\* CORS

\* Middleware global de tratamento de exceções



\*\*Frontend\*\*



\* React

\* Vite

\* Consulta de pagamentos

\* Filtros por status e contrato



\## Fluxo do pagamento



```text

Cliente

&#x20;  │

&#x20;  ▼

POST /webhooks/pagamento

&#x20;  │

&#x20;  ▼

API

&#x20;  │

&#x20;  ▼

ReceberPagamentoHandler

&#x20;  │

&#x20;  ├── Valida status

&#x20;  │

&#x20;  ├── Registra evento

&#x20;  │

&#x20;  ├── Verifica idempotência

&#x20;  │

&#x20;  └── Enfileira pagamento novo

&#x20;          │

&#x20;          ▼

&#x20;  Processamento assíncrono

&#x20;          │

&#x20;          ▼

&#x20;      Persistência

```



\## Idempotência



Cada evento recebido possui um identificador de transação.



Antes de encaminhar o pagamento para processamento, a aplicação verifica se o evento já foi registrado.



Isso evita que uma mesma transação seja processada novamente quando o webhook for reenviado.



\## API



\### Receber pagamento



```http

POST /webhooks/pagamento

X-Api-Key: {sua-api-key}

Content-Type: application/json

```



Exemplo:



```json

{

&#x20; "idTransacao": "TRX-001",

&#x20; "idContrato": "CTR-001",

&#x20; "valor": 150.75,

&#x20; "dataPagamento": "2026-08-14T10:30:00",

&#x20; "status": "Sucesso"

}

```



\### Consultar pagamentos



```http

GET /webhooks/pagamento

X-Api-Key: {sua-api-key}

```



Também é possível utilizar filtros por status e contrato.



\## Frontend



O frontend foi desenvolvido utilizando \*\*React + Vite\*\*.



A aplicação apresenta os pagamentos recebidos e permite consultar os registros utilizando filtros.



Para executar:



```powershell

cd frontend

npm install

npm run dev

```



Por padrão:



```text

http://localhost:5173

```



\## Executando a API



Na raiz do projeto:



```powershell

dotnet restore

dotnet build

dotnet run --project src/Sabemi.PaymentWebhook.Api

```



A API utiliza HTTPS durante o desenvolvimento.



\## Banco de dados



O projeto utiliza \*\*SQL Server\*\* através do Entity Framework Core.



A string de conexão deve ser configurada no ambiente local da aplicação.



Não são incluídas credenciais reais no repositório.



\## Testes



Os testes automatizados utilizam \*\*xUnit\*\*.



Para executar:



```powershell

dotnet test

```



Estado atual:



```text

9 testes

9 aprovados

0 falhas

```



\## Tecnologias



\### Backend



\* C#

\* .NET 10

\* ASP.NET Core

\* MediatR

\* Entity Framework Core

\* SQL Server

\* xUnit



\### Frontend



\* React

\* Vite

\* JavaScript



\### Conceitos aplicados



\* Clean Architecture

\* CQRS

\* Mediator

\* Dependency Injection

\* Repository Pattern

\* Idempotência

\* Processamento assíncrono

\* Background Worker

\* API Key Authentication

\* Testes automatizados



\## Objetivo do projeto



Projeto desenvolvido como demonstração prática de arquitetura, desenvolvimento backend com .NET, integração entre API e frontend, processamento assíncrono e aplicação de boas práticas de engenharia de software.



