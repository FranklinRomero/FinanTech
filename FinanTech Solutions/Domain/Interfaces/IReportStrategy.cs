using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Enums;

namespace FinanTech_Solutions.Domain.Interfaces;

public interface IReportStrategy
{
    UserType SupportedUserType { get; }
    IEnumerable<ReportSection> ProcessData(IEnumerable<FinancialRecord> records);
}
