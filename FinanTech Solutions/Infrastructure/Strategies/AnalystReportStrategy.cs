using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Strategies;

public sealed class AnalystReportStrategy : IReportStrategy
{
    public UserType SupportedUserType => UserType.Analyst;

    public IEnumerable<ReportSection> ProcessData(IEnumerable<FinancialRecord> records)
    {
        var list = records.ToList();
        var sections = new List<ReportSection>();

        foreach (var group in list.GroupBy(r => r.AccountCode).OrderBy(g => g.Key))
        {
            var lines = group
                .OrderBy(r => r.Timestamp)
                .Select(r =>
                    $"{r.Timestamp:yyyy-MM-dd HH:mm} | {r.TransactionType,-6} | " +
                    $"{r.Amount,12:F2} {r.CurrencyCode} | {r.Description}")
                .ToList();

            sections.Add(new ReportSection(
                $"Account: {group.Key}",
                lines.AsReadOnly(),
                new Dictionary<string, object>
                {
                    ["AccountCode"] = group.Key,
                    ["Count"]       = group.Count()
                }));
        }

        var statLines = list
            .GroupBy(r => r.AccountCode)
            .OrderBy(g => g.Key)
            .Select(g =>
            {
                var amounts = g.Select(r => r.Amount).ToList();
                var count   = amounts.Count;
                var sum     = amounts.Sum();
                var avg     = amounts.Average();
                var sorted  = amounts.Order().ToList();
                var median  = count % 2 == 0
                    ? (sorted[count / 2 - 1] + sorted[count / 2]) / 2m
                    : sorted[count / 2];
                return $"{g.Key} | Count:{count} | Sum:{sum:F2} | Avg:{avg:F2} | Median:{median:F2}";
            })
            .ToList();

        sections.Add(new ReportSection(
            "Statistical Summary",
            statLines.AsReadOnly(),
            new Dictionary<string, object>()));

        return sections;
    }
}
