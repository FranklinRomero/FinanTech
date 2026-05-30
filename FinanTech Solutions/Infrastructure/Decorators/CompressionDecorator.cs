using FinanTech_Solutions.Domain.Entities;

namespace FinanTech_Solutions.Infrastructure.Decorators;

public sealed class CompressionDecorator : ReportContentDecoratorBase
{
    public override string EnhancementKey => "Compression";

    protected override Report ApplyEnhancement(Report report)
    {
        report.Metadata["Compression"] = "GZip [STUB — content not compressed]";
        return report;
    }
}
