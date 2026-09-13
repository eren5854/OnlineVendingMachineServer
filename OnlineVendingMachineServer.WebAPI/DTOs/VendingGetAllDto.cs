namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record VendingGetAllDto(
    Guid VendingId,
    string VendingName,
    bool VendingStatus,
    string? VendingImage,
    string VendingSerialNumber,
    bool IsActive
);