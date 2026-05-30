using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;
using FinanTech_Solutions.Domain.ValueObjects;

namespace FinanTech_Solutions.Infrastructure.Delivery;

public sealed class ApiReportDelivery : IReportDelivery
{
    public DeliveryChannel Channel => DeliveryChannel.Api;

    public Task<byte[]?> DeliverAsync(byte[] content, string fileName, DeliveryContext context)
        => Task.FromResult<byte[]?>(content);
}
