using Microsoft.AspNetCore.Identity;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Middlewares;

public static class ExtensionMiddleware
{
    public static void CreateAdmin(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            if (!userManager.Users.Any(p => p.Email == "admin@gmail.com"))
            {
                AppUser user = new()
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                };
                userManager.CreateAsync(user, "Password123*").Wait();
            }
        }
    }
}