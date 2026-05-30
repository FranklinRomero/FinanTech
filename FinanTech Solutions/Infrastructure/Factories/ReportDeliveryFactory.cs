using FinanTech_Solutions.Application.Interfaces;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Factories;

public sealed class ReportDeliveryFactory(IEnumerable<IReportDelivery> deliveries) : IReportDeliveryFactory
{
    public IReportDelivery Create(DeliveryChannel channel) =>
        deliveries.FirstOrDefault(d => d.Channel == channel)
        ?? throw new NotSupportedException($"No delivery registered for channel '{channel}'.");
}
