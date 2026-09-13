namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record LoginRequestDto(
    string UsernameOrEmail,
    string Password
);