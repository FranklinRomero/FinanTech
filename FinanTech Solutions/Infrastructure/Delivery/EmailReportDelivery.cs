using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;
using FinanTech_Solutions.Domain.ValueObjects;

namespace FinanTech_Solutions.Infrastructure.Delivery;

public sealed class EmailReportDelivery(ILogger<EmailReportDelivery> logger) : IReportDelivery
{
    public DeliveryChannel Channel => DeliveryChannel.Email;

    public Task<byte[]?> DeliverAsync(byte[] content, string fileName, DeliveryContext context)
    {
        logger.LogInformation("[EMAIL STUB] Sending {FileName} ({Bytes} bytes) to {Email}",
            fileName, content.Length, context.RecipientEmail);
        return Task.FromResult<byte[]?>(null);
    }
}
