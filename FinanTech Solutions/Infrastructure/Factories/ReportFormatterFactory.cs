using FinanTech_Solutions.Application.Interfaces;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Factories;

public sealed class ReportFormatterFactory(IEnumerable<IReportFormatter> formatters) : IReportFormatterFactory
{
    public IReportFormatter Create(OutputFormat format) =>
        formatters.FirstOrDefault(f => f.SupportedFormat == format)
        ?? throw new NotSupportedException($"No formatter registered for format '{format}'.");
}
