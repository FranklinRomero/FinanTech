namespace FinanTech_Solutions.Domain.Entities;

public record ReportSection(
    string Title,
    IReadOnlyList<string> Lines,
    IReadOnlyDictionary<string, object> Data
);
