using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(p => p.ProductName).IsRequired().HasMaxLength(150);
        builder.Property(p => p.ProductDescription).HasMaxLength(500);
        builder.Property(p => p.ProductPrice).HasColumnType("decimal(18,2)");

        // Eğer Base 'Entity' sınıfınızda IsDeleted varsa aktif edebilirsiniz:
        builder.HasQueryFilter(filter => !filter.IsDeleted);
    }
}