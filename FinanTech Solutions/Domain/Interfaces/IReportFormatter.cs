using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;

namespace FinanTech_Solutions.Domain.Interfaces;

public interface IReportFormatter
{
    OutputFormat SupportedFormat { get; }
    string ContentType { get; }
    byte[] Format(Report report);
}
