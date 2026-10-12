# 🌿 Carbon API — Gestão de Sustentabilidade e Emissões de Carbono (ESG)

API RESTful desenvolvida em **.NET Core 8** para gestão de sustentabilidade empresarial,
monitoramento de emissões de carbono e controle de iniciativas de redução e compensação
de gases de efeito estufa (GEE).

---

## 👨‍🎓 Informações Acadêmicas

| Campo | Informação |
|---|---|
| **Instituição** | FIAP |
| **Curso** | Análise e Desenvolvimento de Sistemas |
| **Disciplina** | Desenvolvimento de APIs |
| **Aluno** | Gabriel Oliver |

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Versão | Finalidade |
|---|---|---|
| **.NET Core** | 8.0 | Framework principal |
| **ASP.NET Core Web API** | 8.0 | Desenvolvimento dos endpoints REST |
| **Entity Framework Core** | 8.0 | ORM para acesso ao banco de dados |
| **Oracle Database** | 23ai Free | Banco de dados relacional |
| **JWT Bearer** | 8.0 | Autenticação e autorização |
| **BCrypt.Net** | 4.0.3 | Hash de senhas |
| **FluentValidation** | 11.3 | Validação de dados |
| **Swagger/OpenAPI** | 6.5 | Documentação da API |
| **xUnit** | 2.6.2 | Framework de testes unitários |
| **Moq** | 4.20 | Mock para testes |
| **FluentAssertions** | 6.12 | Assertivas nos testes |
| **Docker** | - | Containerização |
| **Docker Compose** | - | Orquestração dos containers |

---

## 🏗️ Arquitetura — MVVM

O projeto segue estritamente o padrão **MVVM (Model-View-ViewModel)**
com clara separação de responsabilidades:

CarbonApi/

├── Controllers/          ← Camada de apresentação (endpoints REST)

├── Models/               ← Entidades do domínio

│   └── Enums/            ← Enumeradores

├── ViewModels/           ← DTOs de entrada e saída (ViewModel)

│   ├── Request/          ← Dados de entrada

│   └── Response/         ← Dados de saída

├── Repositories/         ← Acesso ao banco de dados

│   └── Interfaces/       ← Contratos dos repositories

├── Services/             ← Regras de negócio

│   └── Interfaces/       ← Contratos dos services

├── Data/                 ← DbContext e configurações do EF Core

├── Migrations/           ← Migrações do banco de dados

├── Middleware/           ← Tratamento global de exceções

└── Exceptions/           ← Exceções customizadas

---

## 📋 Endpoints Disponíveis

### 🔐 Autenticação
| Método | Endpoint | Acesso | Descrição |
|---|---|---|---|
| `POST` | `/api/v1/auth/login` | Público | Autenticar e obter token JWT |

### 🏢 Empresas
| Método | Endpoint | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/v1/empresas-sustentaveis` | Autenticado | Listar empresas com paginação |
| `GET` | `/api/v1/empresas-sustentaveis/{id}` | Autenticado | Buscar empresa por ID |
| `POST` | `/api/v1/empresas-sustentaveis` | Somente ADMIN | Cadastrar nova empresa |

### 🌿 Emissões de Carbono
| Método | Endpoint | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/v1/emissoes-carbono` | Autenticado | Listar emissões com paginação |
| `GET` | `/api/v1/historico-emissoes/{empresaId}` | Autenticado | Histórico por empresa |
| `POST` | `/api/v1/relatorio-emissao` | ADMIN/EMPRESA | Registrar nova emissão |

### 📅 Agendamentos de Redução
| Método | Endpoint | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/v1/agendamento-reducao-carbono` | Autenticado | Listar agendamentos com paginação |
| `GET` | `/api/v1/agendamento-reducao-carbono/{id}` | Autenticado | Buscar agendamento por ID |
| `POST` | `/api/v1/agendamento-reducao-carbono` | ADMIN/EMPRESA | Criar agendamento |
| `PUT` | `/api/v1/agendamento-reducao-carbono/{id}` | ADMIN/EMPRESA | Atualizar agendamento |
| `DELETE` | `/api/v1/agendamento-reducao-carbono/{id}` | ADMIN/EMPRESA | Cancelar agendamento |

### 🌱 Compensação de Carbono
| Método | Endpoint | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/v1/compensacao-carbono` | Autenticado | Listar compensações com paginação |
| `POST` | `/api/v1/compensacao-carbono` | ADMIN/EMPRESA | Cadastrar compensação |

### 📊 Indicadores
| Método | Endpoint | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/v1/indicadores-sustentabilidade` | Autenticado | Indicadores gerais do sistema |
| `GET` | `/api/v1/impacto-mitigacao/{id}` | Autenticado | Impacto estimado de uma ação |

---

## ✅ Pré-requisitos

Instale antes de rodar o projeto:

| Ferramenta | Download |
|---|---|
| **Docker Desktop** | [docker.com/products/docker-desktop](https://docker.com/products/docker-desktop) |
| **.NET SDK 8.0** | [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download/dotnet/8.0) |

---

## 🚀 Como Executar

### 1. Clone ou extraia o projeto
```bash
cd CarbonApi
```

### 2. Crie o arquivo `.env` na raiz (opcional — valores já estão no docker-compose)
```env
ORACLE_PASSWORD=190492
APP_USER=rm562365
APP_USER_PASSWORD=190492
JWT_SECRET=3cfa76ef14937c1c0ea519f8fc057a80fcd04a7420f8e8bcd0a7567c272e007b
```

### 3. Suba os containers com Docker
```bash
docker compose up --build
```

Aguarde aparecer:

esg_oracle_db        | DATABASE IS READY TO USE!

esg_carbon_api_dotnet | Now listening on: http://[::]:8080

### 4. Acesse o Swagger

http://localhost:8080/swagger

---

## 🔐 Como Autenticar

### 1. Faça login com o usuário administrador:

**POST** `http://localhost:8080/api/v1/auth/login`

```json
{
  "email": "admin@esg.com",
  "senha": "admin123"
}
```

### 2. Copie o token da resposta:
```json
{
  "token": "eyJhbGciOiJIUzI1NiJ9...",
  "tipo": "Bearer",
  "email": "admin@esg.com",
  "role": "ADMIN"
}
```

### 3. No Swagger clique em **Authorize 🔒** e insira:

Bearer eyJhbGciOiJIUzI1NiJ9...

---

## 🧪 Testando com Insomnia

### Configuração do Environment

No Insomnia, crie uma nova collection chamada `Carbon API — .NET Core 8` e configure o **Base Environment**:

```json
{
  "base_url": "http://localhost:8080",
  "token": ""
}
```

### Como usar o token

1. Execute o `POST /api/v1/auth/login`
2. Copie o valor do campo `token` da resposta
3. Abra o **Base Environment** e cole na variável `token`
4. Todos os endpoints autenticados já usam `Bearer {{ token }}` automaticamente

---

### 📁 Auth

**POST** `/api/v1/auth/login` — Público
```json
{
  "email": "admin@esg.com",
  "senha": "admin123"
}
```

---

### 📁 Empresas

**GET** `/api/v1/empresas-sustentaveis?page=1&pageSize=10` — Autenticado

**GET** `/api/v1/empresas-sustentaveis/1` — Autenticado

**POST** `/api/v1/empresas-sustentaveis` — Somente ADMIN
```json
{
  "nome": "Solar Tech S.A.",
  "cnpj": "11.222.333/0001-44",
  "setor": "Energia Solar",
  "pais": "Brasil"
}
```

---

### 📁 Emissões de Carbono

**GET** `/api/v1/emissoes-carbono?page=1&pageSize=10` — Autenticado

**GET** `/api/v1/historico-emissoes/1?page=1&pageSize=10` — Autenticado

**POST** `/api/v1/relatorio-emissao` — ADMIN/EMPRESA
```json
{
  "empresaId": 1,
  "quantidadeTco2e": 150.5000,
  "escopo": 1,
  "descricao": "Emissões diretas da frota de veículos",
  "dataEmissao": "2026-06-19T00:00:00"
}
```
> Valores para `escopo`: `1` = Escopo1, `2` = Escopo2, `3` = Escopo3

---

### 📁 Agendamentos de Redução

**GET** `/api/v1/agendamento-reducao-carbono?page=1&pageSize=10` — Autenticado

**GET** `/api/v1/agendamento-reducao-carbono/1` — Autenticado

**POST** `/api/v1/agendamento-reducao-carbono` — ADMIN/EMPRESA
```json
{
  "empresaId": 1,
  "titulo": "Substituição de Frota Diesel por Elétrica",
  "descricao": "Substituição gradual de 30% da frota por veículos elétricos",
  "metaReducaoTco2e": 50.0000,
  "dataInicio": "2026-07-01T00:00:00",
  "dataFimPrevista": "2026-12-31T00:00:00"
}
```

**PUT** `/api/v1/agendamento-reducao-carbono/1` — ADMIN/EMPRESA
```json
{
  "titulo": "Substituição de Frota Diesel — Revisado",
  "descricao": "Meta ampliada para 50% da frota substituída",
  "metaReducaoTco2e": 75.0000,
  "status": 2,
  "dataFimPrevista": "2027-06-30T00:00:00"
}
```
> Valores para `status`: `1` = Pendente, `2` = EmAndamento, `3` = Concluido, `4` = Cancelado

**DELETE** `/api/v1/agendamento-reducao-carbono/1` — ADMIN/EMPRESA
> Retorna `204 No Content`

---

### 📁 Compensação de Carbono

**GET** `/api/v1/compensacao-carbono?page=1&pageSize=10` — Autenticado

**POST** `/api/v1/compensacao-carbono` — ADMIN/EMPRESA
```json
{
  "empresaId": 1,
  "tipo": 2,
  "quantidadeTco2e": 30.0000,
  "descricao": "Plantio de 1000 árvores nativas no Cerrado brasileiro",
  "certificadoUrl": "https://certificados.esg.com.br/ref-2026-001",
  "dataCompensacao": "2026-06-19T00:00:00"
}
```
> Valores para `tipo`: `1` = CreditoCarbono, `2` = Reflorestamento, `3` = EnergiaRenovavel, `4` = Outro

---

### 📁 Indicadores

**GET** `/api/v1/indicadores-sustentabilidade` — Autenticado

**GET** `/api/v1/impacto-mitigacao/1` — Autenticado

---

### Ordem recomendada de execução

```
1.  POST   /auth/login                        → salvar token
2.  POST   /empresas-sustentaveis             → criar empresa
3.  GET    /empresas-sustentaveis             → listar empresas
4.  GET    /empresas-sustentaveis/1           → buscar por ID
5.  POST   /relatorio-emissao                 → registrar emissão
6.  GET    /emissoes-carbono                  → listar emissões
7.  GET    /historico-emissoes/1              → histórico empresa
8.  POST   /agendamento-reducao-carbono       → criar agendamento
9.  GET    /agendamento-reducao-carbono       → listar agendamentos
10. PUT    /agendamento-reducao-carbono/1     → atualizar
11. POST   /compensacao-carbono               → cadastrar compensação
12. GET    /compensacao-carbono               → listar compensações
13. GET    /indicadores-sustentabilidade      → ver totais
14. GET    /impacto-mitigacao/1               → ver impacto
15. DELETE /agendamento-reducao-carbono/1     → cancelar
```

## 🗄️ Banco de Dados

As tabelas são criadas automaticamente pelo **Entity Framework Core Migrations**
ao subir a aplicação via Docker.

### Tabelas criadas:
| Tabela | Descrição |
|---|---|
| `TB_EMPRESA` | Empresas cadastradas no sistema |
| `TB_EMISSAO_CARBONO` | Registros de emissões de CO2 |
| `TB_AGENDAMENTO_REDUCAO` | Iniciativas de redução de emissões |
| `TB_COMPENSACAO_CARBONO` | Iniciativas de compensação de carbono |
| `TB_USUARIO` | Usuários do sistema |

### Dados iniciais inseridos automaticamente:
| Dado | Valor |
|---|---|
| Empresa | EcoTech Brasil |
| Usuário admin | admin@esg.com / admin123 |

---

## 🧪 Testes Unitários

O projeto possui **10 testes unitários** com **xUnit** cobrindo todos os controllers,
validando o retorno do status code `200 OK`.

### Executar os testes:
```bash
dotnet test
```

### Resultado esperado:

Aprovado! – Com falha: 0, Aprovado: 10, Ignorado: 0, Total: 10

---

## 🛡️ Segurança

| Recurso | Implementação |
|---|---|
| **Autenticação** | JWT Bearer Token |
| **Hash de senhas** | BCrypt |
| **Autorização por roles** | ADMIN, EMPRESA |
| **Tratamento de exceções** | Middleware global |
| **Respostas de erro padronizadas** | `ErrorResponse` |

---

## 📦 Comandos Úteis

| Comando | Descrição |
|---|---|
| `docker compose up --build` | Sobe Oracle + API |
| `docker compose up -d` | Sobe em background |
| `docker compose down` | Para tudo |
| `docker logs esg_carbon_api_dotnet -f` | Logs da API |
| `docker logs esg_oracle_db -f` | Logs do Oracle |
| `dotnet test` | Roda os testes |
| `dotnet ef migrations add NomeMigration` | Nova migration |
| `dotnet ef database update` | Aplica migrations |