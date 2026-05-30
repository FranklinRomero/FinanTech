namespace FinanTech_Solutions.Application.DTOs;

public record ReportResult(
    Guid ReportId,
    bool Success,
    string Message,
    byte[]? InlineContent,
    string? ContentType,
    IReadOnlyList<string> AppliedEnhancements,
    DateTimeOffset GeneratedAt
);
