namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record GoogleLoginResponseDto(
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpires);
