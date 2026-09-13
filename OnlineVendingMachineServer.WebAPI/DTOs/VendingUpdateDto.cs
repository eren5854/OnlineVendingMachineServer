namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record VendingUpdateDto(
    Guid Id,
    string VendingName,
    string? VendingLocation,
    string? VendingType,
    bool VendingStatus,
    string? VendingDescription,
    IFormFile? VendingImage,
    string VendingSerialNumber,
    short VendingSlotCount,
    bool IsActive
);