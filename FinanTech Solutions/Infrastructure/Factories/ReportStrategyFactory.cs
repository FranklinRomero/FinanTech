using FinanTech_Solutions.Application.Interfaces;
using FinanTech_Solutions.Domain.Enums;
using FinanTech_Solutions.Domain.Interfaces;

namespace FinanTech_Solutions.Infrastructure.Factories;

public sealed class ReportStrategyFactory(IEnumerable<IReportStrategy> strategies) : IReportStrategyFactory
{
    public IReportStrategy Create(UserType userType) =>
        strategies.FirstOrDefault(s => s.SupportedUserType == userType)
        ?? throw new NotSupportedException($"No strategy registered for user type '{userType}'.");
}
