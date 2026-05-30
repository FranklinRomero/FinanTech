using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.ValueObjects;

namespace FinanTech_Solutions.Domain.Interfaces;

public interface IReportDelivery
{
    DeliveryChannel Channel { get; }
    Task<byte[]?> DeliverAsync(byte[] content, string fileName, DeliveryContext context);
}
