namespace FinanTech_Solutions.Domain.Entities;

public class Report
{
    public Guid ReportId { get; init; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset GeneratedAt { get; init; } = DateTimeOffset.UtcNow;
    public List<ReportSection> Sections { get; set; } = [];
    public Dictionary<string, string> Metadata { get; set; } = [];
    public List<string> Enhancements { get; set; } = [];
}
