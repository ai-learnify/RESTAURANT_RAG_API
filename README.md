# RestaurantApi

Reference project for STEP's AI module (lectures 1–12): an ASP.NET Core Web API for the restaurant **Supra**, extended step by step with AI features (Microsoft.Extensions.AI + Gemini).

## Run

Requirements: .NET 10 SDK, Docker, `dotnet-ef` (`dotnet tool install -g dotnet-ef`).

```bash
docker compose up -d                 # PostgreSQL 17 on localhost:5432
dotnet ef database update            # schema + seed (menu, tables, bookings for tomorrow)
dotnet run                           # http://localhost:5171
```

- Swagger UI: http://localhost:5171/swagger
- Demo requests: `requests.http` (grouped by lecture)

From lecture 2 the Gemini key is needed — only in User Secrets, never in a tracked file:

```bash
dotnet user-secrets init
dotnet user-secrets set "Gemini:ApiKey" "<your key>"
```

## Lectures (git tags)

| Tag | State |
|---|---|
| `lecture-01` | Starting point: Web API, PostgreSQL, EF Core, seed, `GET api/menu`, `GET api/bookings` |
| `lecture-02` | First call and prompts |
| `lecture-03` | DI and structured output |
| `lecture-04` | Tools (function calling) |
| `lecture-05` | Pipeline and middleware |
| `lecture-06` | Streaming |
| `lecture-07` | History and context |
| `lecture-08` | Resilience and cost |
| `lecture-09` | Text embeddings |
| `lecture-10` | pgvector + EF Core |
| `lecture-11` | Simple RAG |
| `lecture-12` | RAG optimisation and evaluation |

```bash
git checkout lecture-06              # state at the start of lecture 7
git diff lecture-06 lecture-07       # what is written during lecture 7
```

Deviations from the lecture material: `NOTES.md`.
