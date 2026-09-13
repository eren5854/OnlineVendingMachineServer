namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record ResetPasswordRequestDto(
    Guid AppUserId,
    string NewPassword,
    string ConfirmNewPassword);