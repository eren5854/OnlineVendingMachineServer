using Microsoft.AspNetCore.Identity;

namespace OnlineVendingMachineServer.WebAPI.Models;

public sealed class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = false;
        IsActive = true;
    }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}
