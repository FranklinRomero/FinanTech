using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Application.Interfaces;

public interface IReportDeliveryFactory
{
    IReportDelivery Create(DeliveryChannel channel);
}
