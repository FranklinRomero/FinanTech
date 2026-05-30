using FinanTech_Solutions.Domain.Entities;

namespace FinanTech_Solutions.Infrastructure.Decorators;

public sealed class HeaderDecorator : ReportContentDecoratorBase
{
    public override string EnhancementKey => "Header";

    protected override Report ApplyEnhancement(Report report)
    {
        var lines = new List<string>
        {
            "FinanTech Solutions S.A.",
            $"Report: {report.Title}",
            $"Report ID: {report.ReportId}",
            $"Generated: {report.GeneratedAt:yyyy-MM-dd HH:mm:ss} UTC",
            "CONFIDENTIAL — FOR INTERNAL USE ONLY"
        };

        report.Sections.Insert(0, new ReportSection(
            "[HEADER]",
            lines.AsReadOnly(),
            new Dictionary<string, object>()));

        return report;
    }
}
