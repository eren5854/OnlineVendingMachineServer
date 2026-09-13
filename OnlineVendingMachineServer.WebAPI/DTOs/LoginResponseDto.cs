namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record LoginResponseDto(
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpires);
