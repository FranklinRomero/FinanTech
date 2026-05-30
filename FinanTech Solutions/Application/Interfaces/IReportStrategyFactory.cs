using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Application.Interfaces;

public interface IReportStrategyFactory
{
    IReportStrategy Create(UserType userType);
}
