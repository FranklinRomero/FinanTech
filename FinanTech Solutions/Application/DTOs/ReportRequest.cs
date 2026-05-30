using FinanTech_Solutions.Domain.Enums;

namespace FinanTech_Solutions.Application.DTOs;

public record ReportRequest(
    UserType UserType,
    OutputFormat Format,
    DeliveryChannel Channel,
    IReadOnlyList<string> Enhancements,
    string RecipientEmail,
    string SharedFolderPath,
    string RequestingUserId
);
