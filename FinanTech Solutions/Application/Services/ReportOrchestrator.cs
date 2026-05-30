using FinanTech_Solutions.Application.DTOs;
using FinanTech_Solutions.Application.Interfaces;
using FinanTech_Solutions.Domain.Interfaces;
using FinanTech_Solutions.Domain.ValueObjects;
using FinanTech_Solutions.Infrastructure.DataSources;

namespace FinanTech_Solutions.Application.Services;

public sealed class ReportOrchestrator(
    FinancialDataRepository dataRepository,
    IReportStrategyFactory strategyFactory,
    IReportFormatterFactory formatterFactory,
    IReportDeliveryFactory deliveryFactory,
    IReportBuilder builder,
    IEnumerable<IReportContentDecorator> decorators
) : IReportOrchestrator
{
    public async Task<ReportResult> GenerateAsync(ReportRequest request)
    {
        try
        {
            var records  = await dataRepository.GetAllAsync();
            var strategy = strategyFactory.Create(request.UserType);
            var sections = strategy.ProcessData(records);

            builder
                .WithTitle($"Financial Report — {request.UserType} — {DateTimeOffset.UtcNow:yyyy-MM-dd}")
                .WithSections(sections)
                .WithMetadata("RequestedBy", request.RequestingUserId)
                .WithMetadata("UserType",    request.UserType.ToString())
                .WithMetadata("Format",      request.Format.ToString())
                .WithMetadata("GeneratedAt", DateTimeOffset.UtcNow.ToString("O"));

            foreach (var key in request.Enhancements)
            {
                var decorator = decorators.FirstOrDefault(d => d.EnhancementKey == key);
                if (decorator is not null)
                    builder.ApplyDecorator(decorator);
            }

            var report    = builder.Build();
            var formatter = formatterFactory.Create(request.Format);
            var bytes     = formatter.Format(report);
            var fileName  = $"report_{report.ReportId}.{Extension(request.Format)}";
            var context   = new DeliveryContext(request.RecipientEmail, request.SharedFolderPath, request.RequestingUserId);
            var delivery  = deliveryFactory.Create(request.Channel);
            var inline    = await delivery.DeliverAsync(bytes, fileName, context);

            return new ReportResult(
                report.ReportId,
                true,
                "Report generated successfully.",
                inline,
                formatter.ContentType,
                report.Enhancements.AsReadOnly(),
                report.GeneratedAt);
        }
        catch (NotSupportedException ex)
        {
            return new ReportResult(Guid.Empty, false, ex.Message, null, null, [], DateTimeOffset.UtcNow);
        }
    }

    private static string Extension(Domain.Enums.OutputFormat format) => format switch
    {
        Domain.Enums.OutputFormat.Pdf   => "pdf",
        Domain.Enums.OutputFormat.Excel => "xlsx",
        Domain.Enums.OutputFormat.Csv   => "csv",
        _                               => "bin"
    };
}
