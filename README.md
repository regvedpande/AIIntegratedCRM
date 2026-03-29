# AI Integrated CRM — Enterprise Edition

> Production-ready, AI-powered CRM built with **.NET 8**, **Clean Architecture**, **CQRS/MediatR**, and **Blazor WebAssembly**

---

## Features

### CRM Modules
| Module | Description |
|--------|-------------|
| **Leads** | AI-scored lead management with source tracking and one-click conversion |
| **Contacts** | Full contact profiles with account relationships and activity history |
| **Accounts** | Company management with parent-child hierarchy and financial data |
| **Opportunities** | Visual Kanban pipeline with AI win-probability predictions |
| **Activities** | Call, email, meeting tracking with AI meeting summarization |
| **Support Tickets** | Priority-based ticketing with SLA, comment threads, auto-assignment |
| **Workflow Automation** | Rule-based automation engine triggered by CRM events |

### AI Features — Powered by Claude (Anthropic)
- **Lead Scoring** — AI-generated 0–100 scores with reasoning and key conversion factors
- **Opportunity Win Prediction** — Stage-aware win probability with actionable recommendations
- **Natural Language Query** — Ask questions about CRM data in plain English
- **Email Generation** — Professional sales emails crafted in any tone
- **Meeting Summarization** — Automatic extraction of action items from transcripts
- **Semantic Search** — Vector-embedding similarity search across contacts

### Architecture Highlights
- Clean Architecture (Domain → Application → Infrastructure → Presentation)
- Strict CQRS with MediatR pipeline behaviours (Logging, Validation, Performance)
- Domain-Driven Design — aggregates, value objects, domain events
- Multi-tenancy with full tenant isolation at the database level
- Soft-delete + complete audit trail on every entity
- JWT authentication with refresh-token rotation
- Role-based authorization (Admin, Manager, Sales, Support)
- Redis distributed caching
- RabbitMQ event bus (MassTransit-ready)
- Structured logging (Serilog) + health checks
- API rate limiting (sliding window, 100 req/min)

---

## Architecture

```
┌──────────────────────────────────────────────────────────────┐
│              Blazor WebAssembly UI  (port 8080)              │
│   Dashboard | Leads | Pipeline | AI Assistant | Tickets      │
└──────────────────────────┬───────────────────────────────────┘
                           │  HTTP/REST  (JWT Bearer)
┌──────────────────────────▼───────────────────────────────────┐
│           ASP.NET Core 8 Web API  (port 5000)                │
│   Controllers ──► MediatR ──► CQRS Handlers                  │
│   Middleware: Auth · Rate Limit · Exception Handling         │
└────────┬──────────────┬────────────────┬─────────────────────┘
         │              │                │
  ┌──────▼──────┐ ┌─────▼──────┐ ┌──────▼──────────┐
  │ SQL Server  │ │   Redis    │ │  Anthropic API  │
  │ (EF Core 8) │ │  (Cache)   │ │  (AI Features)  │
  └─────────────┘ └────────────┘ └─────────────────┘
```

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Blazor WebAssembly (.NET 8) |
| API | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Database | Microsoft SQL Server 2022 |
| Caching | Redis 7 (StackExchange.Redis) |
| Messaging | RabbitMQ 3.13 + MassTransit 8 |
| AI / ML | Anthropic Claude (`claude-sonnet-4-6`) |
| Auth | JWT Bearer + Refresh Tokens (BCrypt) |
| CQRS | MediatR 12 |
| Validation | FluentValidation 11 |
| Mapping | AutoMapper 13 |
| Logging | Serilog |
| Testing | xUnit 2.9 + Moq 4.20 + FluentAssertions 6 |
| Containers | Docker + Docker Compose |
| CI/CD | GitHub Actions |

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Anthropic API Key](https://console.anthropic.com)

---

## Quick Start — Docker (recommended)

```bash
# 1. Clone
git clone https://github.com/regvedpande/AIIntegratedCRM.git
cd AIIntegratedCRM

# 2. Set your Anthropic API key
#    Windows PowerShell:
$env:ANTHROPIC_API_KEY = "sk-ant-api03-..."
#    Linux / macOS:
export ANTHROPIC_API_KEY="sk-ant-api03-..."

# 3. Launch everything
docker-compose up -d

# 4. Open the app (wait ~30 s for SQL Server to initialise)
# Blazor UI  -> http://localhost:8080
# REST API   -> http://localhost:5000
# Swagger    -> http://localhost:5000/swagger
# RabbitMQ   -> http://localhost:15672  (crm_user / crm_pass)
```

### Demo Credentials
| Field | Value |
|-------|-------|
| Email | `admin@demo.com` |
| Password | `Admin@123` |

---

## Manual Development Setup

```bash
# 1. Start only infrastructure containers
docker-compose up -d sqlserver redis rabbitmq

# 2. Store your API key via user-secrets
cd src/AIIntegratedCRM.API
dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-api03-..."

# 3. Apply EF Core migrations (creates DB + seeds demo data automatically)
dotnet ef database update \
  --project ../AIIntegratedCRM.Infrastructure \
  --startup-project .

# 4. Run the API
dotnet run --project src/AIIntegratedCRM.API
# -> https://localhost:7000/swagger

# 5. Run the Blazor UI (new terminal)
dotnet run --project src/AIIntegratedCRM.Blazor
# -> https://localhost:7173
```

---

## Project Structure

```
AIIntegratedCRM/
├── src/
│   ├── AIIntegratedCRM.Domain/          # Entities, ValueObjects, Events, Interfaces
│   ├── AIIntegratedCRM.Application/     # CQRS handlers, DTOs, validators, behaviors
│   ├── AIIntegratedCRM.Infrastructure/  # EF Core, Repositories, AI/Cache/Token services
│   ├── AIIntegratedCRM.API/             # REST controllers, middleware, Program.cs
│   └── AIIntegratedCRM.Blazor/          # Blazor WebAssembly pages, layout, services
├── tests/
│   └── AIIntegratedCRM.UnitTests/       # xUnit + Moq + FluentAssertions
├── docker-compose.yml
├── docker-compose.override.yml
├── init-db.sql                          # Extra indexes + stored procedures
├── .env.example
└── .github/workflows/ci-cd.yml
```

---

## API Reference

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `/api/v1/auth/register` | Public | Register new tenant + admin user |
| `POST` | `/api/v1/auth/login` | Public | Authenticate, receive JWT |
| `GET` | `/api/v1/leads` | Bearer | Paginated, filtered lead list |
| `POST` | `/api/v1/leads` | Bearer | Create lead |
| `POST` | `/api/v1/leads/{id}/score` | Bearer | AI score a lead |
| `GET` | `/api/v1/opportunities` | Bearer | Pipeline opportunities |
| `PATCH` | `/api/v1/opportunities/{id}/stage` | Bearer | Move to new stage |
| `POST` | `/api/v1/activities/{id}/summarize` | Bearer | AI meeting summary |
| `POST` | `/api/v1/ai/query` | Bearer | Natural language CRM query |
| `POST` | `/api/v1/ai/generate-email` | Bearer | AI email generation |
| `GET` | `/api/v1/dashboard/stats` | Bearer | KPIs + pipeline summary |
| `GET` | `/health` | Public | Dependency health check |

Full Swagger UI: **http://localhost:5000/swagger**

---

## Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string | Yes |
| `ConnectionStrings__Redis` | Redis `host:port` | Yes |
| `JwtSettings__SecretKey` | JWT signing secret (>= 32 chars) | Yes |
| `JwtSettings__Issuer` | JWT issuer | Yes |
| `JwtSettings__Audience` | JWT audience | Yes |
| `Anthropic__ApiKey` | Get one at console.anthropic.com | Yes (AI) |

Copy `.env.example` to `.env` and fill in values before `docker-compose up`.

---

## Running Tests

```bash
dotnet test tests/AIIntegratedCRM.UnitTests/ --verbosity normal
```

---

## Contributing

1. Fork the repo
2. `git checkout -b feature/my-feature`
3. `git commit -m "feat: description"`
4. `git push origin feature/my-feature`
5. Open a Pull Request against `main`

---

## Author

**Regved Pande** — Full Stack .NET Developer
GitHub: [@regvedpande](https://github.com/regvedpande)

---

## License

[MIT](LICENSE) © 2025 Regved Pande
