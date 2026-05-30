using System.Text;
using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Formatters;

public sealed class ExcelReportFormatter : IReportFormatter
{
    public OutputFormat SupportedFormat => OutputFormat.Excel;
    public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public byte[] Format(Report report)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[EXCEL STUB]");
        sb.AppendLine($"Title\t{report.Title}");
        sb.AppendLine($"Report ID\t{report.ReportId}");
        sb.AppendLine($"Generated\t{report.GeneratedAt:O}");
        sb.AppendLine();

        foreach (var (key, value) in report.Metadata)
            sb.AppendLine($"[META]\t{key}\t{value}");

        sb.AppendLine();

        foreach (var section in report.Sections)
        {
            sb.AppendLine($"SECTION\t{section.Title}");
            foreach (var line in section.Lines)
                sb.AppendLine($"\t{line}");
            sb.AppendLine();
        }

        if (report.Enhancements.Count > 0)
        {
            sb.AppendLine("ENHANCEMENTS");
            foreach (var e in report.Enhancements)
                sb.AppendLine($"\t{e}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
