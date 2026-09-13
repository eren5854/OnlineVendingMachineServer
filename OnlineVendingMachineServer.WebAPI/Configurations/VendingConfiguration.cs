using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineVendingMachineServer.WebAPI.Models;
namespace OnlineVendingMachineServer.WebAPI.Configurations;

public sealed class VendingConfiguration : IEntityTypeConfiguration<Vending>
{
    public void Configure(EntityTypeBuilder<Vending> builder)
    {
        builder.ToTable("Vendings");

        builder.Property(v => v.VendingName).IsRequired().HasMaxLength(150);
        builder.Property(v => v.VendingLocation).HasMaxLength(250);
        builder.Property(v => v.VendingType).HasMaxLength(50);
        builder.Property(v => v.VendingDescription).HasMaxLength(500);
        builder.Property(v => v.VendingImage).HasMaxLength(500);
        builder.Property(v => v.VendingSerialNumber).IsRequired().HasMaxLength(100);

        builder.HasIndex(v => v.VendingSerialNumber).IsUnique();

        builder.HasQueryFilter(filter => !filter.IsDeleted);
    }
}
