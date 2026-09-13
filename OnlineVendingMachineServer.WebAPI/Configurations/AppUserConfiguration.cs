using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Configurations;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users");
        builder.Property(p => p.UserName).IsRequired().HasMaxLength(256);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(256);
        builder.Property(p => p.PhoneNumber).HasMaxLength(20);
        builder.HasQueryFilter(filter => !filter.IsDeleted);
    }
}
