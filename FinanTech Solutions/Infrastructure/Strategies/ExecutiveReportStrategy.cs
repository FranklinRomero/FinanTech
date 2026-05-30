using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Strategies;

public sealed class ExecutiveReportStrategy : IReportStrategy
{
    public UserType SupportedUserType => UserType.Executive;

    public IEnumerable<ReportSection> ProcessData(IEnumerable<FinancialRecord> records)
    {
        var list = records.ToList();

        var totalCredits = list.Where(r => r.TransactionType == "Credit").Sum(r => r.Amount);
        var totalDebits  = list.Where(r => r.TransactionType == "Debit").Sum(r => r.Amount);
        var netPosition  = totalCredits - totalDebits;

        var summaryLines = new List<string>
        {
            $"Total Credits : {totalCredits:C2}",
            $"Total Debits  : {totalDebits:C2}",
            $"Net Position  : {netPosition:C2}",
            $"Period        : {list.Min(r => r.Timestamp):yyyy-MM-dd} to {list.Max(r => r.Timestamp):yyyy-MM-dd}"
        };

        var topAccounts = list
            .GroupBy(r => r.AccountCode)
            .Select(g => (Code: g.Key, Total: g.Sum(r => r.Amount)))
            .OrderByDescending(x => x.Total)
            .Take(5)
            .Select(x => $"{x.Code} : {x.Total:C2}")
            .ToList();

        return
        [
            new ReportSection(
                "Executive Summary",
                summaryLines.AsReadOnly(),
                new Dictionary<string, object>
                {
                    ["TotalCredits"] = totalCredits,
                    ["TotalDebits"]  = totalDebits,
                    ["NetPosition"]  = netPosition
                }),
            new ReportSection(
                "Top Accounts by Volume",
                topAccounts.AsReadOnly(),
                new Dictionary<string, object>())
        ];
    }
}
