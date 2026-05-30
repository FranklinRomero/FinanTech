# FinanTech Solutions — Report System Architecture

**GitHub:** `https://github.com/FranklinRomero/FinanTech`

---

## 1. Overview

Internal system for generating, transforming, and delivering automated financial indicator reports.

- **Stack:** C# 13 · .NET 9 · ASP.NET Core Web API
- **Architecture:** Clean Architecture (Domain → Application → Infrastructure → Presentation)
- **Patterns:** Strategy · Decorator · Factory Method · Builder · Repository

No GUI. Stubs replace real PDF rendering, email sending, and file I/O so the focus stays on design.

---

## 2. Folder Structure

```
FinanTech Solutions/
├── Domain/
│   ├── Entities/        Pure business objects — no external dependencies
│   ├── Enums/           Discriminators for strategies, formatters, delivery channels
│   ├── Interfaces/      Contracts owned by the domain (IReportStrategy, IReportBuilder, etc.)
│   └── ValueObjects/    Immutable value objects (DeliveryContext)
├── Application/
│   ├── DTOs/            Request/response shapes that cross layer boundaries
│   ├── Interfaces/      Application-level contracts (IReportOrchestrator, factory interfaces)
│   └── Services/        ReportOrchestrator — wires all steps together
├── Infrastructure/
│   ├── Strategies/      Concrete data-processing algorithms per user type
│   ├── Decorators/      Optional report enhancements, stacked at runtime
│   ├── Formatters/      PDF / Excel / CSV renderers
│   ├── Delivery/        Email / SharedFolder / API delivery channels
│   ├── Builders/        ReportBuilder — fluent report construction
│   ├── Factories/       Resolves concrete implementations by enum discriminator
│   └── DataSources/     FinancialDataRepository — seeded stub data
└── Presentation/
    └── Controllers/     ReportsController — single POST endpoint
```

**Dependency rule:** each layer may only reference inward layers. Presentation → Application → Domain. Infrastructure implements Domain/Application interfaces but is only referenced from the composition root (`Program.cs`).

---

## 3. Design Patterns

| Pattern | Location | Problem Solved | OCP Benefit |
|---|---|---|---|
| **Strategy** | `Infrastructure/Strategies/` | Executive, Auditor, and Analyst each need a different projection of the same `FinancialRecord` data. Encoding this with `if/switch` in the orchestrator violates SRP and OCP simultaneously. | Add a new user type = add one class implementing `IReportStrategy` + one DI line. Zero existing files change. |
| **Decorator** | `Infrastructure/Decorators/` | Header, Watermark, Encryption, and Compression are independent, optional, and composable. Inheritance would produce 2⁴ = 16 subclasses for every combination. Decorator lets the caller stack them in any order at runtime. | Add a new enhancement = add one class extending `ReportContentDecoratorBase` + one DI line. |
| **Factory Method** | `Infrastructure/Factories/` | The orchestrator must not `new` concrete formatters or delivery channels — doing so would create a hard compile-time dependency that breaks DIP. Each factory holds the single LINQ expression that maps an enum value to the matching implementation. | Add a new format or delivery channel = add one class + one DI line. The factory discovers it automatically via `IEnumerable<T>` injection. |
| **Builder** | `Infrastructure/Builders/` | `Report` requires a title, sections (from the strategy), metadata key-values, and an ordered decorator pipeline. A long constructor is fragile and unreadable. Builder provides a fluent protocol that the orchestrator always follows in the same sequence regardless of user type. | New report fields are added to the builder without changing callers. |
| **Repository** | `Infrastructure/DataSources/` | Isolates data access behind a simple async interface. The orchestrator depends on `FinancialDataRepository` directly (acceptable for a small project); swapping to a real database requires changing only this class. | Swap stub for real DB = zero changes in Application or Domain layers. |

---

## 4. Full Request Flow

```
POST /api/reports/generate
        │
        ▼
ReportsController.Generate(ReportRequest)
        │
        ▼
ReportOrchestrator.GenerateAsync(request)
        │
        ├─[1] FinancialDataRepository.GetAllAsync()
        │      → IEnumerable<FinancialRecord>  (20 seeded records)
        │
        ├─[2] ReportStrategyFactory.Create(UserType)          [STRATEGY]
        │      Selects one of:
        │        ExecutiveReportStrategy  → KPI summary + top accounts
        │        AuditorReportStrategy    → full ledger + flagged transactions
        │        AnalystReportStrategy    → per-account detail + statistics
        │      → IEnumerable<ReportSection>
        │
        ├─[3] ReportBuilder fluent chain                      [BUILDER]
        │      .WithTitle(...)
        │      .WithSections(sections)
        │      .WithMetadata("RequestedBy", ...)
        │      .WithMetadata("UserType", ...)
        │      ...
        │
        ├─[4] For each key in request.Enhancements            [DECORATOR]
        │      Resolved from injected IEnumerable<IReportContentDecorator>
        │      by EnhancementKey match; caller controls order.
        │        "Header"      → prepends header section
        │        "Watermark"   → sets Metadata["Watermark"]
        │        "Encryption"  → sets Metadata["Encryption"]
        │        "Compression" → sets Metadata["Compression"]
        │
        ├─[5] ReportBuilder.Build()
        │      Runs decorator pipeline in registration order → Report
        │
        ├─[6] ReportFormatterFactory.Create(OutputFormat)     [FACTORY METHOD]
        │      Selects: PdfReportFormatter | ExcelReportFormatter | CsvReportFormatter
        │      formatter.Format(report) → byte[]
        │
        ├─[7] ReportDeliveryFactory.Create(DeliveryChannel)   [FACTORY METHOD]
        │      Selects: EmailReportDelivery | SharedFolderReportDelivery | ApiReportDelivery
        │      delivery.DeliverAsync(bytes, fileName, context)
        │        Email/Folder → logs stub message, returns null
        │        Api          → returns bytes as inline content
        │
        └─[8] Returns ReportResult to controller
               Channel == Api  → File(...) download response
               Otherwise       → Ok(result) JSON response
```

---

## 5. Demo via API

Run the project (`F5` in Visual Studio or `dotnet run`). Base URL: `https://localhost:7224`

**1 — Executive PDF via API with Header + Watermark**
```http
POST https://localhost:7224/api/reports/generate
Content-Type: application/json

{
  "userType": 0,
  "format": 0,
  "channel": 2,
  "enhancements": ["Header", "Watermark"],
  "recipientEmail": "",
  "sharedFolderPath": "",
  "requestingUserId": "user-exec-001"
}
```
Expected: HTTP 200, file download with `Content-Type: application/pdf`.

**2 — Auditor Excel via Email with full decorator stack**
```http
POST https://localhost:7224/api/reports/generate
Content-Type: application/json

{
  "userType": 1,
  "format": 1,
  "channel": 0,
  "enhancements": ["Header", "Watermark", "Encryption", "Compression"],
  "recipientEmail": "auditor@finantech.com",
  "sharedFolderPath": "",
  "requestingUserId": "user-aud-007"
}
```
Expected: HTTP 200 JSON. App logs show `[EMAIL STUB] Sending report_<guid>.xlsx ...`. `appliedEnhancements: ["Header","Watermark","Encryption","Compression"]`.

**3 — Analyst CSV via Shared Folder**
```http
POST https://localhost:7224/api/reports/generate
Content-Type: application/json

{
  "userType": 2,
  "format": 2,
  "channel": 1,
  "enhancements": [],
  "recipientEmail": "",
  "sharedFolderPath": "C:\\Reports\\Analyst",
  "requestingUserId": "user-ana-003"
}
```
Expected: HTTP 200 JSON. App logs show `[FOLDER STUB] Writing report_<guid>.csv ...`.

**4 — Invalid user type (error path)**
```http
POST https://localhost:7224/api/reports/generate
Content-Type: application/json

{
  "userType": 99,
  "format": 0,
  "channel": 2,
  "enhancements": [],
  "recipientEmail": "",
  "sharedFolderPath": "",
  "requestingUserId": "user-unknown"
}
```
Expected: HTTP 400 with `{ "message": "No strategy registered for user type '99'." }`.

**5 — Decorator order test**  
Send two identical requests except swap the order of enhancements: `["Encryption","Header"]` vs `["Header","Encryption"]`. Verify `appliedEnhancements` in each response preserves the caller-specified order.

All five requests are also pre-configured in `FinanTech Solutions.http`.

---

## 6. Extending the System (Open/Closed Principle)

### Add a new user type (e.g., `ComplianceOfficer`)

1. Add `ComplianceOfficer` to `Domain/Enums/UserType.cs`.
2. Create `Infrastructure/Strategies/ComplianceOfficerReportStrategy.cs` implementing `IReportStrategy`.
3. Add one DI line in `Program.cs`:
   ```csharp
   builder.Services.AddTransient<IReportStrategy, ComplianceOfficerReportStrategy>();
   ```

Zero existing files change.

### Add a new output format (e.g., `Xml`)

1. Add `Xml` to `Domain/Enums/OutputFormat.cs`.
2. Create `Infrastructure/Formatters/XmlReportFormatter.cs` implementing `IReportFormatter`.
3. Register in `Program.cs`: `builder.Services.AddTransient<IReportFormatter, XmlReportFormatter>();`

### Add a new delivery channel (e.g., `Slack`)

1. Add `Slack` to `Domain/Enums/DeliveryChannel.cs`.
2. Create `Infrastructure/Delivery/SlackReportDelivery.cs` implementing `IReportDelivery`.
3. Register in `Program.cs`: `builder.Services.AddTransient<IReportDelivery, SlackReportDelivery>();`

### Add a new decorator (e.g., `DigitalSignature`)

1. Create `Infrastructure/Decorators/DigitalSignatureDecorator.cs` extending `ReportContentDecoratorBase`.
2. Register in `Program.cs`: `builder.Services.AddTransient<IReportContentDecorator, DigitalSignatureDecorator>();`
3. Include `"DigitalSignature"` in the `enhancements` array of any request.

---

## 7. Dependency Injection Map

| Interface / Class | Concrete | Lifetime | Reason |
|---|---|---|---|
| `IReportStrategy` (×3) | `ExecutiveReportStrategy`, `AuditorReportStrategy`, `AnalystReportStrategy` | Transient | Stateless; resolved as `IEnumerable<IReportStrategy>` in factory |
| `IReportStrategyFactory` | `ReportStrategyFactory` | Transient | Lightweight resolver |
| `IReportContentDecorator` (×4) | `HeaderDecorator`, `WatermarkDecorator`, `EncryptionDecorator`, `CompressionDecorator` | Transient | Stateless; resolved as `IEnumerable<IReportContentDecorator>` in orchestrator |
| `IReportFormatter` (×3) | `PdfReportFormatter`, `ExcelReportFormatter`, `CsvReportFormatter` | Transient | Stateless |
| `IReportFormatterFactory` | `ReportFormatterFactory` | Transient | Lightweight resolver |
| `IReportDelivery` (×3) | `EmailReportDelivery`, `SharedFolderReportDelivery`, `ApiReportDelivery` | Transient | Stateless |
| `IReportDeliveryFactory` | `ReportDeliveryFactory` | Transient | Lightweight resolver |
| `IReportBuilder` | `ReportBuilder` | **Transient** | **Stateful** — accumulates title/sections/decorators during construction. Must be fresh per request; Singleton or Scoped would leak state across requests. |
| `FinancialDataRepository` | — | **Singleton** | Immutable seeded data; safe to share across all requests |
| `IReportOrchestrator` | `ReportOrchestrator` | Transient | Coordinates per-request pipeline |
