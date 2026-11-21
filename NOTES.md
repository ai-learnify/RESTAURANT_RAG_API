# Notes — deviations from the lecture material

- L01: EF Core / Npgsql 11.x target net11.0 only — pinned to the latest 10.x (`Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.4, `Microsoft.EntityFrameworkCore.Design` 10.0.15) to stay on .NET 10.
- L01: Swagger = built-in `AddOpenApi()` + `Swashbuckle.AspNetCore.SwaggerUI` (UI only) at `/swagger`.
- L01: seed lives in `Data/DataSeed.cs`; products and tables are seeded with `HasData` (stable Ids in the migration); bookings with `UseSeeding` (dates relative to tomorrow, inserted only when the table is empty, on `dotnet ef database update`). Re-seed fresh bookings: `DELETE FROM "Bookings";` then `dotnet ef database update`.
- L01: `Booking.At` is `timestamptz` (Npgsql default) and all times are UTC (`DateTime.UtcNow`). The restaurant time a guest says ("20:00") is stored as 20:00 UTC — no time zone conversion, kept simple for the course.
- Structure: follows the course spec (feature controllers `AssistantController`, `FeedbackController`, `BookingsController`, `MenuController`, `DocumentsController`, `AiDiagnosticsController`, `EvaluationController`; `Ai/<Area>/` subfolders), not the original single `AiLabController` plan. ⭐ items are not in the main history.
- L01: `ProductsController` (`api/products`) replaced by the lecturer's `MenuController` (`api/menu`) and `BookingsController` (`api/bookings`) so both exist from the starting point.
