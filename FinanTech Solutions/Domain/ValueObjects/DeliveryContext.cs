namespace FinanTech_Solutions.Domain.ValueObjects;

public record DeliveryContext(
    string RecipientEmail,
    string SharedFolderPath,
    string RequestingUserId
);
