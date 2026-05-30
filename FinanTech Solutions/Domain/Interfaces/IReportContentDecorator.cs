using FinanTech_Solutions.Domain.Entities;

namespace FinanTech_Solutions.Domain.Interfaces;

public interface IReportContentDecorator
{
    string EnhancementKey { get; }
    Report Decorate(Report report);
}
