# RevendaPedidos – Backend Challenge

Sistema de processamento e gerenciamento de pedidos para revendas, com arquitetura moderna, mensageria assíncrona e estrutura escalável.

---

## Sumário

- [Sobre o projeto](#sobre-o-projeto)
- [Arquitetura & Técnicas aplicadas](#arquitetura--técnicas-aplicadas)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Como rodar com Docker Compose](#como-rodar-com-docker-compose)
- [APIs & Documentação](#apis--documentação)
- [Testes](#testes)
- [Diretórios principais](#diretórios-principais)
- [Contribuição](#contribuição)

---

## Sobre o projeto

O **RevendaPedidos** é um sistema para gerenciamento de revendas, e pedidos para fornecedora, refletindo boas práticas de DDD, Clean Architecture e mensageria assíncrona. Você pode consultar, inserir ou processar pedidos via API e orquestrar tarefas de processamento usando workers desacoplados.

---

## Arquitetura & Técnicas aplicadas

- **DDD (Domain-Driven Design):** Classes de domínio ricas, encapsulando lógica de negócio e regras.
- **Encapsulamento de entidades:** Coleções expostas apenas como leitura e manipuladas via métodos de domínio.
- **Clean Architecture:** Separação de Domain, Application, Infra e API.
- **Mensageria assíncrona:** Workers consomem eventos da fila RabbitMQ usando MassTransit, desacoplando o processamento da escrita na API.
- **Persistência com EF Core:** Utilizando migrations, relacionamentos e value objects.
- **Docker:** API, Worker, RabbitMQ e SQL Server são orquestrados via Docker Compose.

---

## Tecnologias Utilizadas

- [.NET 8+](https://dotnet.microsoft.com/)
- **Entity Framework Core**
- **MassTransit** (mensageria)
- **RabbitMQ** (fila)
- **SQL Server** (persistência)
- **Docker / Docker Compose**
- **Swagger/OpenAPI** para documentação
- **xUnit** para testes automatizados

---

## Como rodar com Docker Compose

### Pré-requisitos

- [Docker](https://www.docker.com/get-started) e [Docker Compose](https://docs.docker.com/compose/install/) instalados.

### Suba o ambiente (API, Worker, RabbitMQ e SQL) com:

```bash
docker-compose up --build
```

Aguarde os containers subirem. Os serviços principais estarão disponíveis:

- **API:** [http://localhost:5000](http://localhost:5000) – Swagger disponível em `/swagger`
- **RabbitMQ UI:** [http://localhost:15672](http://localhost:15672)  
  Login: `guest` | Senha: `guest`
- **SQL Server:** `localhost:1433`  
  Usuário: `sa` | Senha: `Strong!Passw0rd`
- **Worker:** executa em background consumindo a fila `fila_pedidos`

> ⚠️ As portas podem ser trocadas conforme o `docker-compose.yml`.

### Parar o ambiente

```bash
docker-compose down
```

---

## APIs & Documentação

Após subir o ambiente, acesse:

- [http://localhost:5000/swagger](http://localhost:5000/swagger) – documentação e testes das rotas HTTP

## Testes

Para rodar os testes localmente:

```bash
dotnet test
```

---

### Estrutura de Pastas

- **/src/RevendaPedidos.Api**  
  API HTTP principal construída em .NET (exposição dos endpoints REST).
- **/src/RevendaPedidos.Application**  
  Contratos, interfaces e casos de uso da aplicação (Application Layer).
- **/src/RevendaPedidos.Application.Impl**  
  Implementação dos serviços e casos de uso especificados em Application.
- **/src/RevendaPedidos.DI**  
  Configuração central de injeções de dependências (Dependency Injection).
- **/src/RevendaPedidos.Domain**  
  Camada de domínio: entidades, agregados, value objects e regras de negócio (DDD).
- **/src/RevendaPedidos.Infrastructure**  
  Infraestrutura: persistência (EF Core), mensageria, repositórios e integrações.
- **/src/RevendaPedidos.Shared**  
  Objetos e utilitários compartilhados, helpers, exceções, tipos comuns.
- **/src/RevendaPedidos.Worker**  
  Worker/serviço de background para processamento assíncrono e consumo de filas RabbitMQ.
- **/tests/RevendaPedidos.Test**  
  Testes automatizados (unitários e de integração).



