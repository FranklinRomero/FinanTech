using FinanTech_Solutions.Domain.Entities;

namespace FinanTech_Solutions.Infrastructure.Decorators;

public sealed class WatermarkDecorator : ReportContentDecoratorBase
{
    public override string EnhancementKey => "Watermark";

    protected override Report ApplyEnhancement(Report report)
    {
        report.Metadata["Watermark"] = "CONFIDENTIAL — FinanTech Solutions";
        return report;
    }
}
