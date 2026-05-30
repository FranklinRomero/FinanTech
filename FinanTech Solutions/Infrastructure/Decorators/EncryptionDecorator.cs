using FinanTech_Solutions.Domain.Entities;

namespace FinanTech_Solutions.Infrastructure.Decorators;

public sealed class EncryptionDecorator : ReportContentDecoratorBase
{
    public override string EnhancementKey => "Encryption";

    protected override Report ApplyEnhancement(Report report)
    {
        report.Metadata["Encryption"] = "AES-256 [STUB — content not encrypted]";
        return report;
    }
}
