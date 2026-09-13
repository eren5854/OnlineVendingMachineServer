using ED.Result;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineVendingMachineServer.WebAPI.DTOs;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Services;

public sealed class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<Result<string>> ChangePassword(ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.AppUserId.ToString());
        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        if (request.CurrentPassword == request.NewPassword)
            return Result<string>.Failure("Yeni şifre mevcut şifreden farklı olmalı");


        if (request.NewPassword != request.ConfirmNewPassword)
            return Result<string>.Failure("Yeni şifre ve yeni şifre tekrarı eşleşmiyor");


        IdentityResult result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            return Result<string>.Failure("Hata!! Şifre değiştirme başarısız!!");

        return Result<string>.Succeed("Şifre değiştirme başarılı.");
    }

    public async Task<Result<GoogleLoginResponseDto>> GoogleLogin(GoogleLoginRequestDto request, CancellationToken cancellationToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>
            {
                //"838164728497-v5vnto6us970bv8kmgfjtjo8r57nl9iv.apps.googleusercontent.com"
            }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
        AppUser? user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        bool result = user != null;
        if (user is null)
        {
            user = await userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                user = new()
                {
                    UserName = request.Email,
                    Email = request.Email,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                };
                var identityResult = await userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
            else
            {
                return Result<GoogleLoginResponseDto>.Failure("Bu e-posta adresi zaten kayıtlı. Lütfen giriş yapın.");
            }
        }
        if (!result)
        {
            return Result<GoogleLoginResponseDto>.Failure("Hata!! Kayıt başarısız.");
        }

        await userManager.AddLoginAsync(user, info);

        var loginResponse = await jwtProvider.CreateToken(user);

        var googleLoginResponse = new GoogleLoginResponseDto(
            loginResponse.Token,
            loginResponse.RefreshToken,
            loginResponse.RefreshTokenExpires);

        return Result<GoogleLoginResponseDto>.Succeed(googleLoginResponse);
    }

    public async Task<Result<LoginResponseDto>> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        string emailOrUsername = request.UsernameOrEmail;
        AppUser? appUser = await userManager
            .Users
            .FirstOrDefaultAsync(p => p.Email == emailOrUsername ||
                                    p.UserName == emailOrUsername, cancellationToken);
        if (appUser is null)
            return Result<LoginResponseDto>.Failure("Kullanıcı bulunamadı");

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(appUser, request.Password, true);
        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = appUser.LockoutEnd - DateTimeOffset.UtcNow;
            if (timeSpan is not null)
                return Result<LoginResponseDto>.Failure($"Şifre 5 defa hatalı girildi! Lütfen {Math.Ceiling(timeSpan.Value.TotalSeconds)} saniye bekleyiniz.");
            else
                return Result<LoginResponseDto>.Failure("Wait 3 minutes");
        }
        if (!signInResult.Succeeded)
        {
            return Result<LoginResponseDto>.Failure("Şifre Yanlış");
        }

        var loginResponse = await jwtProvider.CreateToken(appUser);
        appUser.LastLogin = DateTime.UtcNow;
        await userManager.UpdateAsync(appUser);
        return loginResponse;
    }

    public Task<Result<string>> ResetPassword(ResetPasswordRequestDto request, CancellationToken cancellationToken)
    {
        //var currentUser = await generalService.GetCurrentUserAsync(cancellationToken);
        //if (currentUser is null)
        //    return Result<string>.Failure("Kullanıcı doğrulanamadı.");
        //if (currentUser.Role != UserRoleEnum.Admin)
        //    return Result<string>.Failure("Bu işlemi gerçekleştirmek için admin yetkisine sahip olmalısınız.");
        //AppUser? appUser = await userManager.FindByIdAsync(request.AppUserId.ToString());
        //if (appUser is null) return Result<string>.Failure("Kullanıcı bulunamadı");
        //if (request.NewPassword != request.ConfirmNewPassword) return Result<string>.Failure("Şifreler eşleşmiyor");
        //string resetToken = await userManager.GeneratePasswordResetTokenAsync(appUser);
        //IdentityResult result = await userManager.ResetPasswordAsync(appUser, resetToken, request.NewPassword);
        //if (!result.Succeeded)
        //{
        //    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //    return Result<string>.Failure($"Şifre sıfırlama başarısız: {errors}");
        //}
        //return Result<string>.Succeed("Şifre admin tarafından başarıyla sıfırlandı.");
        throw new NotImplementedException();
    }
}
