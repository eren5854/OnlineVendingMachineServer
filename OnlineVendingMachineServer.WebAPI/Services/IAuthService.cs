using ED.Result;
using OnlineVendingMachineServer.WebAPI.DTOs;

namespace OnlineVendingMachineServer.WebAPI.Services;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> Login(LoginRequestDto request, CancellationToken cancellationToken);
    Task<Result<GoogleLoginResponseDto>> GoogleLogin(GoogleLoginRequestDto request, CancellationToken cancellationToken);
    Task<Result<string>> ResetPassword(ResetPasswordRequestDto request, CancellationToken cancellationToken);
    Task<Result<string>> ChangePassword(ChangePasswordRequestDto request, CancellationToken cancellationToken);
}
