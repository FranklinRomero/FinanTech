using FinanTech_Solutions.Domain.Entities;

namespace FinanTech_Solutions.Domain.Interfaces;

public interface IReportBuilder
{
    IReportBuilder WithTitle(string title);
    IReportBuilder WithSections(IEnumerable<ReportSection> sections);
    IReportBuilder WithMetadata(string key, string value);
    IReportBuilder ApplyDecorator(IReportContentDecorator decorator);
    Report Build();
}
