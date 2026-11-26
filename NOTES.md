# Notes — deviations from the lecture material

- L01: EF Core / Npgsql 11.x target net11.0 only — pinned to the latest 10.x (`Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.4, `Microsoft.EntityFrameworkCore.Design` 10.0.15) to stay on .NET 10.
- L01: Swagger = built-in `AddOpenApi()` + `Swashbuckle.AspNetCore.SwaggerUI` (UI only) at `/swagger`.
- L01: seed lives in `Data/DataSeed.cs`; products and tables are seeded with `HasData` (stable Ids in the migration); bookings with `UseSeeding` (dates relative to tomorrow, inserted only when the table is empty, on `dotnet ef database update`). Re-seed fresh bookings: `DELETE FROM "Bookings";` then `dotnet ef database update`.
- L01: `Booking.At` is `timestamptz` (Npgsql default) and all times are UTC (`DateTime.UtcNow`). The restaurant time a guest says ("20:00") is stored as 20:00 UTC — no time zone conversion, kept simple for the course.
- Structure: follows the course spec (feature controllers `AssistantController`, `FeedbackController`, `BookingsController`, `MenuController`, `DocumentsController`, `AiDiagnosticsController`, `EvaluationController`; `Ai/<Area>/` subfolders), not the original single `AiLabController` plan. ⭐ items are not in the main history.
- L01: `ProductsController` (`api/products`) replaced by the lecturer's `MenuController` (`api/menu`) and `BookingsController` (`api/bookings`) so both exist from the starting point.
- L02: DTOs in `Contracts/<Feature>/`, one record per file (not `Dtos/<Feature>/XxxDtos.cs`); names `AskRequest(Message, …)`, `AskResponse`, `SentimentRequest(Message)`, `SentimentResponse(Text)`.
- L02: service registration and the pipeline moved to `Extensions/ServiceExtensions.cs` / `ApplicationExtensions.cs`; `Program.cs` only calls `AddServices` / `UseApplication`.
- L02: `ask`, `chat` and `sentiment` all in `AiDiagnosticsController` (`api/diagnostics/ai/…`), not in separate `AssistantController` / `FeedbackController`.
- L02: DI from the start — `AddChatClient` + `IChatClient` in the constructor already in L02 (students know DI); the "new Client in the constructor" step is skipped. L03 only adds the `?? throw` config check.
- ⭐ bonus code stays in the code with a `// Bonus – …` comment, instead of being left out of the history.
- L02: on 2026-11-26 `gemini-3.8-flash` returned 503 "model is currently experiencing high demand" several times in a row — a real example for L08 (resilience, fallback model).
