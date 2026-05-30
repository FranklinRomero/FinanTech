using FinanTech_Solutions.Application.DTOs;

namespace FinanTech_Solutions.Application.Interfaces;

public interface IReportOrchestrator
{
    Task<ReportResult> GenerateAsync(ReportRequest request);
}
