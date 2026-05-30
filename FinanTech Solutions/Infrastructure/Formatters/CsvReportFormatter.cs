using System.Text;
using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Formatters;

public sealed class CsvReportFormatter : IReportFormatter
{
    public OutputFormat SupportedFormat => OutputFormat.Csv;
    public string ContentType => "text/csv";

    public byte[] Format(Report report)
    {
        var sb = new StringBuilder();
        sb.AppendLine("\"Section\",\"Line\"");

        foreach (var section in report.Sections)
        {
            foreach (var line in section.Lines)
            {
                var s = section.Title.Replace("\"", "\"\"");
                var l = line.Replace("\"", "\"\"");
                sb.AppendLine($"\"{s}\",\"{l}\"");
            }
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
