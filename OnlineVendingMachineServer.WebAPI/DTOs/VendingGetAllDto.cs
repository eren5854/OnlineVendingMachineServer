namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record VendingGetAllDto(
    Guid Id,
    string VendingName,
    bool VendingStatus,
    string? VendingImage,
    string VendingSerialNumber,
    bool IsActive
);