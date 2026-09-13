namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record ChangePasswordRequestDto(
    Guid AppUserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword);
