#  MindTrack — Gerenciador Inteligente Baseado em Estresse

API RESTful construída em .NET8 para gerenciamento de usuários, metas, tarefas e métricas de bem-estar (batimentos cardíacos, HRV, sono e Stress Score). A API serve como back-end da plataforma MindTrack, que combina biometria + IA para reorganizar automaticamente a agenda do usuário de acordo com seu nível de estresse.

##  Visão Geral do Projeto

O MindTrack é um sistema que utiliza dados fisiológicos do usuário para identificar seu nível de estresse e reorganizar sua rotina de forma inteligente.

##  Objetivo

Ajudar o usuário a ter dias mais equilibrados, reorganizando tarefas flexíveis quando o estresse estiver alto, preservando compromissos importantes.

##  Como funciona
Coleta biométrica (via Health Connect):
- Frequência cardíaca (FC)
- HRV (quando disponível)
- Indicadores básicos de sono

Cálculo do Stress Score (0–100):
- Algoritmo que combina variações pessoais e dados de saúde do dia.

Classificação inteligente de tarefas (IA):
- Sugestão automática de nível de dificuldade, peso emocional e prioridade recomendada.

Reorganização automática da agenda:
- Tarefas fixas não são alteradas
- Tarefas flexíveis são reordenadas com base no estado fisiológico do usuário

Interface do app (mobile):
- Mostra Stress Score do dia
- Lista reorganizada de tarefas
- Justificativa da IA (ex.: “Seu nível de estresse está alto hoje”)

> Esta API provê todos os recursos de CRUD, autenticação, métricas e cálculo de Stress Score consumidos pelo app mobile.

##  Tecnologias Utilizadas

- .NET8 (C#12)
- Entity Framework Core
- JWT Bearer Authentication
- FluentValidation
- AutoMapper
- Swagger / OpenAPI com versionamento
- Health Checks
- Middleware customizado de exceções

##  Estrutura Principal

- `Program.cs` — configura serviços (Auth, CORS, Swagger, Versioning, HealthChecks, Middleware etc.)
- `Infrastructure/Data/MindTrackContext.cs` — EF Core, relacionamentos e entidades
- `Application/Service` — regras de negócio (ex.: `AuthService`, `MetaService`)
- `Application/Mapper` — perfis do AutoMapper
- `Domain/Models` — entidades (`User`, `Meta`, `Tarefa`, `HeartMetric`, `StressScore`)
- `Migrations` — histórico das migrations do EF Core

##  Requisitos

- .NET8 SDK
- Banco SQL Server (ou outro provedor configurado)
- VS Code ou Visual Studio2022 (opcional)

##  Configuração

Edite `MindTrack/appsettings.json` ou use variáveis de ambiente para:
- `ConnectionStrings:SqlServerConnection`
- `JwtSettings:SecretKey`

Exemplo mínimo:

```json
{
 "ConnectionStrings": {
 "SqlServerConnection": "Server=.;Database=MindTrackDb;Trusted_Connection=True;"
 },
 "JwtSettings": {
 "SecretKey": "uma-chave-muito-secreta-e-grande"
 }
}
```

Em produção: use Key Vault / secrets, nunca versionar sua secret key.

## ▶️ Executando localmente

Via CLI:

1. Restaurar pacotes

 `dotnet restore`

2. Criar banco / aplicar migrations

 `dotnet ef database update --project MindTrack`

3. Executar

 `dotnet run --project MindTrack`

Via Visual Studio:

- Abrir solução
- Setar `MindTrack` como projeto de inicialização
- Build + Run

##  Migrations

Criar migration:

`dotnet ef migrations add NomeDaMigration --project MindTrack`

Aplicar migration:

`dotnet ef database update --project MindTrack`

##  Documentação Swagger

- UI: `/swagger`
- Documentos: `/swagger/v1/swagger.json` e `/swagger/v2/swagger.json`
- Configuração para enviar JWT via Header `Authorization: Bearer {token}`
- Versionamento ativo (URL + header)

##  Versionamento da API

- URL: `/api/v{version}`
- Header: `X-Api-Version`

##  Endpoints principais

Autenticação

- `POST /api/v{version}/auth/login`

 Body:
 ```json
 {
 "email": "user@example.com",
 "password": "123456"
 }
 ```

CRUDs principais

- `/api/v{version}/users`
- `/api/v{version}/metas`
- `/api/v{version}/tarefas`
- `/api/v{version}/heartmetrics`
- `/api/v{version}/stressscores`

## ❤Health Checks

- `GET /health`
- `GET /health/details` — inclui ping ao banco + estado geral da API

##  Modelo de Banco & Relacionamentos

- `User` → `Metas` (Cascade)
- `Meta` → `Tarefas` (SetNull)
- `User` → `Tarefas` (Cascade)
- `User` → `HeartMetrics` (Cascade)
- `User` → `StressScores` (Restrict)
- `StressScore` → `HeartMetric` (FK opcional)

##  Boas Práticas para Produção

- Use HTTPS + CORS restritivo
- Proteja chaves e conexões (Key Vault / secrets)
- Configure logs estruturados
- Habilite validação completa do JWT (Issuer/Audience)
- Implemente refresh tokens (roadmap futuro)
- Considere APM (Application Insights)

##  Contribuição

- Abra issues com bugs ou sugestões
- Envie PRs seguindo convenções do repositório
- Rode migrations + testes antes de enviar
---

Se quiser que eu gere algum dos itens acima, diga qual e eu preparo os arquivos correspondentes.
