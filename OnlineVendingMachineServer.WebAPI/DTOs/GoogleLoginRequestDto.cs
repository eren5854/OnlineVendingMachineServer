namespace OnlineVendingMachineServer.WebAPI.DTOs;

public sealed record GoogleLoginRequestDto(
    string Id,
    string IdToken,
    string Name,
    string FirstName,
    string LastName,
    string Email,
    string PhotoUrl,
    string Provider);
