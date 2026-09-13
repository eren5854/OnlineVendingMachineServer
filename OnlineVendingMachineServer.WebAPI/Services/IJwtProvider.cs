using OnlineVendingMachineServer.WebAPI.DTOs;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Services;

public interface IJwtProvider
{
    Task<LoginResponseDto> CreateToken(AppUser user);
}
