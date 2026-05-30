using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Strategies;

public sealed class AuditorReportStrategy : IReportStrategy
{
    public UserType SupportedUserType => UserType.Auditor;

    public IEnumerable<ReportSection> ProcessData(IEnumerable<FinancialRecord> records)
    {
        var list = records.OrderBy(r => r.Timestamp).ToList();

        var ledgerLines = list
            .Select(r =>
                $"{r.Timestamp:O} | {r.Id} | {r.AccountCode} | {r.TransactionType,-6} | " +
                $"{r.Amount,12:F2} {r.CurrencyCode} | Audit:{r.AuditUserId} | {r.AuditNote ?? "—"}")
            .ToList();

        var flaggedLines = list
            .Where(r => r.Amount > 20_000m)
            .Select(r =>
                $"FLAGGED {r.Id} | {r.AccountCode} | {r.Amount:C2} | " +
                $"Audit:{r.AuditUserId} | Note:{r.AuditNote ?? "none"}")
            .ToList();

        if (flaggedLines.Count == 0)
            flaggedLines.Add("No flagged transactions.");

        return
        [
            new ReportSection(
                "Transaction Ledger",
                ledgerLines.AsReadOnly(),
                new Dictionary<string, object> { ["Count"] = list.Count }),
            new ReportSection(
                "Flagged Transactions (> $20,000)",
                flaggedLines.AsReadOnly(),
                new Dictionary<string, object> { ["FlaggedCount"] = flaggedLines.Count })
        ];
    }
}
