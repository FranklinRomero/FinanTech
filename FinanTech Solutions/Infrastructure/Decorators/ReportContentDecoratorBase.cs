using FinanTech_Solutions.Domain.Entities;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Decorators;

public abstract class ReportContentDecoratorBase : IReportContentDecorator
{
    public abstract string EnhancementKey { get; }

    public Report Decorate(Report report)
    {
        report.Enhancements.Add(EnhancementKey);
        return ApplyEnhancement(report);
    }

    protected abstract Report ApplyEnhancement(Report report);
}
