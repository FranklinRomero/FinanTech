using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Builders;

public sealed class ReportBuilder : IReportBuilder
{
    private string _title = string.Empty;
    private List<ReportSection> _sections = [];
    private readonly Dictionary<string, string> _metadata = [];
    private readonly List<IReportContentDecorator> _pipeline = [];

    public IReportBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public IReportBuilder WithSections(IEnumerable<ReportSection> sections)
    {
        _sections = [.. sections];
        return this;
    }

    public IReportBuilder WithMetadata(string key, string value)
    {
        _metadata[key] = value;
        return this;
    }

    public IReportBuilder ApplyDecorator(IReportContentDecorator decorator)
    {
        _pipeline.Add(decorator);
        return this;
    }

    public Report Build()
    {
        var report = new Report
        {
            Title    = _title,
            Sections = _sections,
            Metadata = new Dictionary<string, string>(_metadata)
        };

        foreach (var decorator in _pipeline)
            report = decorator.Decorate(report);

        return report;
    }
}
