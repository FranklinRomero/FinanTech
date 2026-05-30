namespace FinanTech_Solutions.Domain.Entities;

public record FinancialRecord(
    Guid Id,
    DateTimeOffset Timestamp,
    string AccountCode,
    string Description,
    decimal Amount,
    string CurrencyCode,
    string TransactionType,
    string AuditUserId,
    string? AuditNote
);
