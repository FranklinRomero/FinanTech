using System.Text;
using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Formatters;

public sealed class PdfReportFormatter : IReportFormatter
{
    public OutputFormat SupportedFormat => OutputFormat.Pdf;
    public string ContentType => "application/pdf";

    public byte[] Format(Report report)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[PDF STUB]");
        sb.AppendLine($"Title      : {report.Title}");
        sb.AppendLine($"Report ID  : {report.ReportId}");
        sb.AppendLine($"Generated  : {report.GeneratedAt:O}");
        sb.AppendLine();

        foreach (var (key, value) in report.Metadata)
            sb.AppendLine($"[META] {key}: {value}");

        sb.AppendLine();

        foreach (var section in report.Sections)
        {
            sb.AppendLine($"=== {section.Title} ===");
            foreach (var line in section.Lines)
                sb.AppendLine(line);
            sb.AppendLine();
        }

        if (report.Enhancements.Count > 0)
        {
            sb.AppendLine("[Enhancements Applied]");
            foreach (var e in report.Enhancements)
                sb.AppendLine($"  - {e}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
