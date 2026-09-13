namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record VendingGetDto(
    Guid VendingId,
    string VendingName,
    string? VendingLocation,
    string? VendingType,
    bool VendingStatus,
    string? VendingDescription,
    string? VendingImage,
    string VendingSerialNumber,
    short VendingSlotCount,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt
);