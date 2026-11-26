# RestaurantApi — the AI module reference project

This repo is the reference project for STEP's AI module (lectures 1–12). Each lecture adds one step to the same ASP.NET Core Web API. The git history follows the lectures: one or more commits per lecture and a tag at the end (`lecture-01` … `lecture-12`). Other lecturers will use `git checkout lecture-06` to see the exact state a lecture starts from, and `git diff lecture-06 lecture-07` to see what is written during lecture 7.

The code, names and routes must match the course materials (`courses/ai/project/NN.md`) **exactly**. The structure below is the source of truth for this repo.

The owner (Gocha) is writing this himself to practise before teaching.

## How to work with me

- **Talk to me in English.** Code, comments, prompts, data, commit messages and docs are in **English** too.
- **Propose first, then act.** Before each step say in 1–3 lines what you'll change and why; edit files only after I say go. Small steps — one file or one endpoint at a time.
- **Explain briefly as you go** — the reason, not a lecture. I know .NET well; I'm practising the AI parts.
- **Stay in the current lecture.** Never add something that a later lecture introduces (e.g. no structured output in lecture 2, no `CancellationToken` plumbing before lecture 6, no pgvector before lecture 10). Some code is *intentionally* naive at first and fixed later — that's the point of the history.
- **Build after every step** (`dotnet build`). Run and test with the requests in `requests.http` before saying a step is done.
- **I run migrations, docker and git myself.** Never run `dotnet ef` (migrations add / database update), `docker` / `docker compose`, or `git commit` / tag / push — give me the exact command instead.
- At the end of a lecture, propose the commit message and the tag.
- **My changes win.** I'll sometimes restructure or rename things my own way. When I do, don't argue it back toward the spec. Update the docs to match: the structure and plan in this file, a line in `NOTES.md`, and `README.md` / `requests.http` if routes change.
- If the real API of a package differs from the plan below (MEAI and Google.GenAI change often), tell me, pick the current correct API, and add a line to `NOTES.md`.

## Stack

- .NET 10, ASP.NET Core Web API with **Controllers**, Swagger (built-in OpenAPI + Swagger UI)
- PostgreSQL in Docker (`compose.yml`), EF Core + Npgsql, migrations
- `Microsoft.Extensions.AI` (MEAI) + `Google.GenAI` (Gemini) — **Gemini only**
- Later: `Microsoft.Extensions.Http.Resilience`, `Pgvector.EntityFrameworkCore`, `PdfPig`, `Microsoft.Extensions.AI.Evaluation.Quality`
- Use the latest stable package versions; check them with `dotnet add package` rather than from memory.

## Principles

- **Simple and clean.** Logic lives in the controllers. No Clean Architecture, MediatR, VSA, repositories, services-for-everything, Result types or Semantic Kernel. A separate class only when several controllers use it or it is a real service on its own. Explicit beats clever.
- **Realistic, professional names.** No "Lab", "Playground" or toy names.
- **A file appears in its final place from the first lecture** and is never moved or renamed later. No "cleanup" at the end.
- ⭐ = "if there's time left": optional in the lecture. ⭐ code **stays in the code**, marked with a `// Bonus – …` comment above it; the lecturer decides whether to write it live.

## Final structure (lecture where it appears in brackets)

`+` = a file that already exists in the starting point (`lecture-01`); the AI lectures only add to it.

```
Controllers/
  AssistantController       api/assistant
                            GET chat/stream (06)
                            04 tools (CreateChatOptions) · 07 history (ConversationId) · 11 RAG (sources) · 12 Category
  +BookingsController       POST api/bookings/parse, action Parse (03)
  +MenuController           POST api/menu/embeddings (GenerateEmbeddings) · GET api/menu/search?query= (Search)
                            09: vectors in a static in-memory list · 10: in the database (pgvector)
  DocumentsController       POST api/documents (Upload) · GET api/documents/search?query= (11; 12 + category)
  FeedbackController        api/feedback/ai: POST sentiment, action AnalyzeSentiment (03 enum, no few-shot)
  AiDiagnosticsController   api/diagnostics/ai: ask, chat (02) · sentiment ⭐ few-shot (02 only; 03 moves it to FeedbackController)
                            · stream-timing (06) · embed, similarity (09)
  EvaluationController      api/evaluation: retrieval (hit@3, MRR) · groundedness (12)
Ai/
  Middleware/   InputGuardChatClient (05), UsageTrackingChatClient (05 log → 08 database),
                FallbackChatClient (08), AiExceptionHandler (08 ⭐)
  Tools/        RestaurantTools (04: GetMenu, CheckAvailability; 10 ⭐ SearchMenu)
  Chat/         ChatHistoryStore (07), LastTurnsReducer (07 ⭐)
  Embeddings/   EmbeddingInput (09: Query / Document prefixes)
  Documents/    PdfTextReader, TextChunker, DocumentSearch (11: SearchAsync + FormatContext;
                12: SearchAsync becomes hybrid tsvector + RRF, VectorSearchAsync kept for comparison)
  Diagnostics/  ChaosHandler (08)
Entities/       +Product, +Table, +Booking, Message (07), AiUsage (08), Product (10: Embedding, EmbeddingModel),
                Document, Chunk (11; 12: Category, SearchVector)
Data/           +DataContext, +DataSeed — new DbSets: Messages, AiUsages, Documents, Chunks
Contracts/      one record per file: Contracts/<Feature>/<Name>.cs
  Diagnostics/   AskRequest(string Message, float? Temperature, int? MaxOutputTokens), AskResponse (02),
                 ChatRequest(string Message) (02; 07 + Guid? ConversationId; 12 + string? Category = null),
                 ChatReply(string Text) (02; 07 + Guid ConversationId; 11 + List<ChatSource> Sources), ChatSource(int Number, string File, int Page) (11),
                 EmbedRequest/EmbedResponse, SimilarityRequest(First, Second)/SimilarityResponse (09)
  Feedback/      SentimentRequest(string Message), SentimentResponse(string Text) (02 in Diagnostics/, 03 moved here)
  Bookings/      BookingParseRequest(string Message), BookingParseResponse(DateTime? At, int? Guests) (03) — also the model's output schema
  Menu/          MenuMatch(string Name, double Score), EmbeddingsResult(int Count) (09)
  Documents/     DocumentUploaded(int Id, int Chunks), ChunkMatch(File, Page, Content, double Score) (11)
  Evaluation/    GoldenItem, RetrievalReport, GroundednessRequest, GroundednessReport (12)
Enums/          Sentiment { Positive, Negative, Mixed } (03)
Extensions/     ServiceExtensions (AddServices), ApplicationExtensions (UseApplication) — Program.cs stays three lines (02)
golden.json (12)
```

- Fields are added lecture by lecture, as shown in brackets.
- DTOs always live in `Contracts/<Feature>/`, one record per file, never inside a controller.
- `ask` and `chat` live in `AiDiagnosticsController` (02); `sentiment` moves to `FeedbackController` in 03. Where later chat features (tools, history, RAG) go is decided when we get there — update this tree then.

## Decisions that must not come back

- ❌ `AiLabController` or any single big controller. ✅ The feature controllers above.
- ❌ `[DevelopmentOnly]`, a `Filters/` folder. ✅ `AiDiagnosticsController` and `EvaluationController` are ordinary controllers.
- ❌ A `GoldenSet` class. ✅ `golden.json` is read directly in `EvaluationController.Retrieval` (`System.IO.File`).
- ❌ A `MenuIndex` singleton. ✅ 09: a `static` list in `MenuController`; 10: the database.
- ❌ `wwwroot/chat.html` + `UseStaticFiles`. ✅ 06 ⭐: a separate `chat.html` outside the project (Live Server) + a CORS policy.
- ❌ Tool calls or token counts in API responses. ✅ In the log: `Tools called: ...` (04); tokens through `UsageTrackingChatClient` (05/08).
- ❌ Old names: `ChatToolOptions`, `TokenCounterChatClient`, `EmbeddingText`, `PdfText`, `ChatMessageRecord`, `AiUsageRecord`, `DocumentChunk`, `FoundChunk`, `BookingRequest`, `HybridSearchAsync`, `SearchChunksAsync`, `/api/ai-lab/...`.

## Conventions

- `Data/` holds **only** `DataContext` and `DataSeed` (seed data as its own static class). Entities go in `Entities/` (namespace `RestaurantApi.Entities`).
- Entities are plain POCOs — **no** data annotations (`[Column]`, `[Precision]`, `[Key]`…). Column types, precision, indexes and seed are configured in `OnModelCreating` (Fluent API) — including `jsonb`, `vector(768)`, the tsvector column and HNSW/GIN indexes.
- Service registration and the HTTP pipeline live in `Extensions/ServiceExtensions.cs` (`AddServices`) and `Extensions/ApplicationExtensions.cs` (`UseApplication`); `Program.cs` only calls them.
- My style: **primary constructors** (`public sealed class MenuController(DataContext context) : ControllerBase`), no `private readonly` fields; `sealed` classes; `var`; attributes combined on one line (`[ApiController, Route("api/menu")]`); `DataContext` used directly.
- The EF Core context is always named **`DataContext`** (`Data/DataContext.cs`) — never `AppDbContext`.
- Records for DTOs.
- Configuration: `Gemini:ApiKey` **only** in User Secrets (`dotnet user-secrets set "Gemini:ApiKey" "…"`); `Gemini:Model`, `Gemini:LiteModel`, `Gemini:FallbackModel`, `Gemini:EmbeddingModel`, `Rag:MinScore` in `appsettings.json`. Never an environment variable named `GEMINI_API_KEY`, never a key in a tracked file.
- Chat model: **`gemini-3.1-flash-lite`** (`Gemini:LiteModel`) is the active one — `gemini-3.8-flash` (`Gemini:Model`) kept to try first in class; on the free tier it often returns 503 "high demand". Switch back once there's a paid Gemini plan. Embedding model: `gemini-embedding-2`, 768 dimensions.
- Restaurant name in prompts: **Supra**, open 12:00–23:00, prices in GEL.
- All data (seed, prompts, test questions, PDF) in English.

## Files to keep up to date

- `requests.http` — every endpoint with the test inputs of its lecture, grouped by lecture (`### L04 · assistant chat`). Lecturers run the demo from here.
- `NOTES.md` — one line per deviation from the lectures or a mistake found in the lecture material: `L08: Google.GenAI retry option is now called X, not Y`.
- `README.md` — how to run (docker compose up, user-secrets, `dotnet ef database update`, run) and the list of tags.

## Commits and tags

- Commit message: `L04: tools — GetMenu, CheckAvailability`
- End of each lecture: tag `lecture-NN` on the last commit. `lecture-01` = the starting point (lecture 1 has no code).

---

# The plan, lecture by lecture

## 0 · Starting point (state after the API course) → tag `lecture-01`

- Solution `RestaurantApi`, Web API with Controllers, Swagger, `.gitignore` (dotnet), `compose.yml` with **`postgres:17`** and a named volume. *Not* pgvector yet — switching the image is a step of lecture 10.
- EF Core + Npgsql, `Data/DataContext.cs`, `Data/DataSeed.cs`, entities:
  - `Product` — Id, Name, Description, Price
  - `Table` — Id, Number, Seats
  - `Booking` — Id, TableId, At, Guests, Name
- Seed: 15–20 Georgian dishes with **rich English descriptions** (search in lectures 9–10 runs on them), vegetarian and meat, prices 4–30 GEL; 6–8 tables (2, 4, 6 seats); a few bookings for tomorrow evening.
- The lecturer's existing controllers: `MenuController` (`api/menu`, GET list of products) and `BookingsController` (`api/bookings`, GET list of bookings). The AI lectures add actions to them.
- `Program.cs`: `UseExceptionHandler`, `MapControllers`.

## Lecture 2 · First call and prompts → `lecture-02`

- Packages: `Microsoft.Extensions.AI`, `Google.GenAI`. User Secrets with `Gemini:ApiKey`; `Gemini:Model` in `appsettings.json`.
- DI from the start (students already know DI): `ServiceExtensions.AddServices` registers `services.AddChatClient(new Client(apiKey: config["Gemini:ApiKey"]!).AsIChatClient(config["Gemini:Model"]))`; `AiDiagnosticsController(IChatClient chat)`.
- `POST api/diagnostics/ai/ask` — `AskRequest(Message, Temperature, MaxOutputTokens)` → `ChatOptions`; returns `AskResponse(Text, InputTokens, OutputTokens, ReasoningTokens, Reason)` with static `From(ChatResponse)`. Token counts are fine here: it's the diagnostics endpoint.
- `POST api/diagnostics/ai/chat` — `ChatRequest(Message)` → `ChatReply(Text)`. System prompt (`SystemPrompt` constant): Supra assistant, topics (menu, bookings, hours 12:00–23:00), answer form (≤ 3 sentences, guest's language), boundary (decline other topics, never invent prices).
- ⭐ `POST api/diagnostics/ai/sentiment` (`AnalyzeSentiment`) — few-shot: System + 3 User/Assistant pairs (Positive / Negative / Mixed), answer is one word. `SentimentRequest(Message)` → `SentimentResponse(Text)`.
- Test: chat — a normal question, another topic (a poem), a price question; `ask` with Temperature 0 three times; `MaxOutputTokens = 50` → with `gemini-3.8-flash`: empty answer because thinking ate the limit; with the lite model (no thinking): cut-off text, finishReason `length`.

## Lecture 3 · Structured output → `lecture-03`

- No DI step (done in L02) and no `?? throw` config check (not needed).
- `Enums/Sentiment.cs`; new `FeedbackController` (`api/feedback/ai`) · `POST sentiment` (`AnalyzeSentiment`) → `GetResponseAsync<Sentiment>` with only a System message, no few-shot; return `response.Result.ToString()` (not `response.Text` — that's the raw `{"data": …}` JSON). On `Negative` it writes `SMS: notify the manager` — the typed result drives code. The L02 few-shot `sentiment` is removed from `AiDiagnosticsController`; its DTOs move to `Contracts/Feedback/`.
- `BookingsController` · `POST api/bookings/parse` (`Parse`) — `BookingParseRequest(Message)` → `GetResponseAsync<BookingParseResponse>`, where `BookingParseResponse([property: Description("Booking date and time")] DateTime? At, [property: Description("Number of guests")] int? Guests)`. System: extract the booking, "Today is {DateTime.Now:yyyy-MM-dd, dddd}.", "Leave null whatever is not in the message". Validation in C#: `At` null → `BadRequest("Which day and what time?")`, `Hour is < 12 or >= 23` → `BadRequest("We're open 12:00–23:00")`, `Guests` null → `BadRequest("How many of you?")`.
- Test: "There are four of us, tomorrow at 8" · "I'd like a table on Saturday evening" · "How much is khinkali?" · "Two of us, tomorrow at 23:30".

## Lecture 4 · Tools (function calling) → `lecture-04`

- `Ai/Tools/RestaurantTools.cs` (constructor takes `DataContext`):
  - `GetMenu()` → `List<MenuItemInfo(Name, Price)>`, `[Description("Returns the restaurant menu: dish name and price in GEL")]`
  - `CheckAvailability(DateTime at, int guests)` → `AvailabilityInfo(bool Available, int FreeTables)`: tables with enough seats and no booking within ±2 hours. `[Description]` on the method and both parameters. Read-only.
- `ServiceExtensions`: `RestaurantTools` as Scoped; `.UseFunctionInvocation()` on the chat client builder.
- `AssistantController.Chat`: options from `CreateChatOptions()` — `Tools = [AIFunctionFactory.Create(tools.GetMenu), AIFunctionFactory.Create(tools.CheckAvailability)]`; System: "You are a restaurant assistant. Answer briefly. Take menu and free-table data only from the tools, don't make anything up. Today is …". Tool calls are **logged** (`Tools called: ...`), not returned.
- Test: "There are four of us, tomorrow at 8 — do you have a table?" · "How much is lobio?" · "Hi, until what time are you open?" · "Two of us are coming on Saturday at 7. What vegetarian dishes do you have?".

## Lecture 5 · Pipeline and middleware → `lecture-05`

- `.UseLogging()`; `appsettings.Development.json`: `"Microsoft.Extensions.AI": "Debug"` — see two Gemini requests for one tool question.
- `UseDistributedCache` (in-memory distributed cache) as an experiment — see when it returns a stale answer, then remove or keep it off.
- `Ai/Middleware/UsageTrackingChatClient.cs` — `DelegatingChatClient`, logs `Usage` of every call.
- `Ai/Middleware/InputGuardChatClient.cs` — `DelegatingChatClient`, short-circuits a question longer than `MaxLength` with "The question is too long — {MaxLength} characters at most." without calling Gemini.
- Order: `InputGuard` → `UseFunctionInvocation` → `UsageTracking` → `UseLogging`.

## Lecture 6 · Streaming → `lecture-06`

- `AiDiagnosticsController` · `POST api/diagnostics/ai/stream-timing` — `GetStreamingResponseAsync` + `await foreach`, returns the parts with elapsed ms (shows TTFT).
- `AssistantController` · `GET api/assistant/chat/stream` — SSE with `TypedResults.ServerSentEvents(...)` over an async iterator (`yield return`). Test with `curl -N`.
- `CancellationToken` in three places: the action, the iterator (`[EnumeratorCancellation]`), the Gemini call.
- Streaming overrides for `InputGuardChatClient` and `UsageTrackingChatClient`.
- ⭐ `chat.html` outside the project (Live Server, `EventSource`) + a CORS policy — not in the main history.

## Lecture 7 · History and context → `lecture-07`

- `Entities/Message.cs`: Id, ConversationId, Position, Role, `Json` (`jsonb` via Fluent API), CreatedAt. `DbSet<Message> Messages`. Migration.
- `Ai/Chat/ChatHistoryStore.cs`: `LoadAsync(conversationId)` / `AppendAsync(conversationId, messages)`, serializing `ChatMessage` with `AIJsonUtilities.DefaultOptions` + `AllowOutOfOrderMetadataProperties = true` (jsonb reorders keys).
- `ChatRequest` gets `Guid? ConversationId`, `ChatReply` gets `Guid ConversationId` (new one when null). `Chat` loads history, sends `[SystemPrompt(), .. history, question]`, appends the question and `response.Messages`. The System prompt is **not** stored.
- ⭐ `Ai/Chat/LastTurnsReducer.cs` (`IChatReducer`): everything stays in the DB, only the last N go to the model.
- Test: "Four of us tomorrow at 20:00, do you have room?" → "And at 21:00?".

## Lecture 8 · Resilience and cost → `lecture-08`

- `Microsoft.Extensions.Http.Resilience`: a named `HttpClient` with `AddStandardResilienceHandler` (`AttemptTimeout` 40 s); pass it to Google.GenAI and **turn off its own retry** (otherwise retries multiply).
- `Ai/Diagnostics/ChaosHandler.cs` — test only: 2 of every 3 requests return 503. Register only in Development.
- `Ai/Middleware/FallbackChatClient.cs` — `DelegatingChatClient`; on a transient error (`catch (ApiException ex) when …` — 429, 5xx, network, timeout) call `Gemini:FallbackModel`; log `"🛟 {Error} — fallback model"`. Innermost layer.
- `Entities/AiUsage.cs` (Id, At, Model, InputTokens, OutputTokens), `DbSet<AiUsage> AiUsages` + migration; `UsageTrackingChatClient` writes one row per call via `IServiceScopeFactory` (it's a Singleton, `DataContext` is Scoped). SQL for today's cost by model.
- ⭐ `Ai/Middleware/AiExceptionHandler.cs` — not in the main history.

## Lecture 9 · Text embeddings → `lecture-09`

- `AddEmbeddingGenerator(...)` with `gemini-embedding-2`, 768 dimensions.
- `AiDiagnosticsController` · `POST embed` — one text → vector (length + first numbers); `POST similarity` — `SimilarityRequest(First, Second)`: cosine by hand, then `TensorPrimitives.CosineSimilarity`.
- `Ai/Embeddings/EmbeddingInput.cs` — `Query(q)` = `"task: search result | query: {q}"`, `Document(title, text)` = `"title: {title} | text: {text}"`.
- `MenuController`: vectors in a **`static`** list. `POST api/menu/embeddings` (`GenerateEmbeddings`, all products, one `GenerateAsync` call → `EmbeddingsResult(Count)`), `GET api/menu/search?query=` (`Search`) → top 3 `MenuMatch(Name, Score)` above a threshold.
- Test: `ILIKE '%without meat%'` → 0 rows vs `api/menu/search?query=without meat` → lobio, aubergine…

## Lecture 10 · pgvector + EF Core → `lecture-10`

- `compose.yml`: `pgvector/pgvector:pg17` — same major version, same volume.
- `Pgvector.EntityFrameworkCore`; `UseNpgsql(..., o => o.UseVector())`, `HasPostgresExtension("vector")`.
- `Product`: `Vector? Embedding` (`vector(768)` via Fluent API), `string? EmbeddingModel`. Migration.
- `GenerateEmbeddings` — only products with `Embedding == null`, in batches (`Chunk(n)`), saves to the DB. Remove the static list.
- `Search` — `OrderBy(p => p.Embedding!.CosineDistance(query))` with a threshold, then an extra filter (max price) in the same SQL.
- HNSW index with `vector_cosine_ops`; check with `EXPLAIN`.
- ⭐ `RestaurantTools.SearchMenu` — not in the main history.

## Lecture 11 · Simple RAG → `lecture-11`

- First create `rules.pdf` (Word → Save as PDF), 6 sections, one per page: general info, bookings (cancel up to 24 hours before, 30% prepayment for 8+), allergens (walnut…), children and pets (dogs only on the terrace), events (hall "B-2" for 20+, own cake — a 15 GEL service charge), payment (service charge 10%, 5+ people — 12%). The full text is in lecture 11's lecturer notes.
- `PdfPig`. `Ai/Documents/PdfTextReader.cs` — pages with `ContentOrderTextExtractor`, cleanup.
- `Ai/Documents/TextChunker.cs` — by sentences, ~800 characters, overlap of 1 sentence.
- `Entities/Document.cs`, `Entities/Chunk.cs` (DocumentId, Page, Content, Embedding vector(768)) + HNSW; DbSets `Documents`, `Chunks`. Migration.
- `DocumentsController` · `POST api/documents` (`Upload`, `IFormFile`): same file name → delete the old one first; document + all chunks in **one** `SaveChangesAsync`; returns `DocumentUploaded(Id, Chunks)`. `GET api/documents/search?query=` → `ChunkMatch(File, Page, Content, Score)`.
- `Ai/Documents/DocumentSearch.cs`: `SearchAsync(query)` → top 5 above `Rag:MinScore`; `FormatContext` → `[1] (rules.pdf, p. 3)\n…`, or "(No relevant information was found in the sources)".
- `AssistantController.Chat`: the augmented message `Sources: … Question: …` goes to the model, the **original** question goes to the history; System adds: answer rules questions only from the sources, cite `[N]`, otherwise say "I don't have information on this". `ChatReply` gets `List<ChatSource> Sources` (`Number, File, Page`).

## Lecture 12 · RAG optimisation and evaluation → `lecture-12`

- `golden.json` (copied to output): 6 items `{ question, mustContain }` — "How do I cancel a booking?" / "24 hours before", "I'm allergic to nuts" / "walnut", "Can I bring a dog?" / "terrace", "Where is hall B-2?" / "B-2", "Can I bring my own cake?" / "15 GEL", "Service charge for 6 people?" / "12%".
- `EvaluationController` · `POST api/evaluation/retrieval` (`Retrieval`) — reads `golden.json` directly (`System.IO.File`) into `GoldenItem`s; for each question: search, rank of the first chunk containing the phrase → hit@3, MRR, misses → `RetrievalReport`. No LLM call. **Run it and write the numbers in `NOTES.md`.**
- `Chunk.SearchVector` — `NpgsqlTsVector`, `HasGeneratedTsVectorColumn(c => c.SearchVector, "english", c => new { c.Content })` + GIN index.
- `DocumentSearch`: the old vector search is renamed `VectorSearchAsync` (kept for comparison); `SearchAsync` becomes hybrid — vector list + `ToTsQuery("english", …)` list (words only, joined with `|`) → RRF (k = 60) → top 5. **Run the evaluation again**, compare.
- Metadata: `Document.Category`, filter in the same SQL (`SearchAsync(query, category)`); `ChatRequest` gets `string? Category = null`, `GET api/documents/search` gets `category`.
- `POST api/evaluation/groundedness` — `GroundednessRequest` → `GroundednessReport`, `Microsoft.Extensions.AI.Evaluation.Quality`, `GroundednessEvaluator` with a separate judge client.

## Known traps

- L03: `GetResponseAsync<T>` for a non-object `T` (enum) wraps the answer as `{"data": …}` — use `response.Result`, not `response.Text`.
- L03: the model returns `At` with `Z` (`Kind = Utc`) — fine for `timestamptz`, use it as is. Don't call `ToLocalTime()` (shifts the time and Npgsql rejects `Kind = Local` for `timestamptz`).
- L02: the `Model` (appsettings) and `ApiKey` (secrets) merge into one `Gemini` section — read both via `IConfiguration`.
- L07: without `AllowOutOfOrderMetadataProperties` deserializing from jsonb fails.
- L08: Google.GenAI's own retry + the resilience handler = multiplied retries.
- L10: a different PostgreSQL major version can't read the old volume.
- L11: scanned PDFs have no text layer → 0 chunks.
- L12: `to_tsquery` with raw punctuation → syntax error; that's why the tsquery keeps only words.
