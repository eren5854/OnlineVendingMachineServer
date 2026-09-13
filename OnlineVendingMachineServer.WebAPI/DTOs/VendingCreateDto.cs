namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record VendingCreateDto(
    string VendingName,
    string? VendingLocation,
    string? VendingType,
    bool VendingStatus,
    string? VendingDescription,
    IFormFile? VendingImage,
    string VendingSerialNumber,
    short VendingSlotCount
);