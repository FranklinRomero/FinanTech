using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;
using FinanTech_Solutions.Domain.ValueObjects;

namespace FinanTech_Solutions.Infrastructure.Delivery;

public sealed class SharedFolderReportDelivery(ILogger<SharedFolderReportDelivery> logger) : IReportDelivery
{
    public DeliveryChannel Channel => DeliveryChannel.SharedFolder;

    public Task<byte[]?> DeliverAsync(byte[] content, string fileName, DeliveryContext context)
    {
        logger.LogInformation("[FOLDER STUB] Writing {FileName} ({Bytes} bytes) to {Path}",
            fileName, content.Length, context.SharedFolderPath);
        return Task.FromResult<byte[]?>(null);
    }
}
