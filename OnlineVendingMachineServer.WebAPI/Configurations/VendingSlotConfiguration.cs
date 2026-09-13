using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Configurations;

public sealed class VendingSlotConfiguration : IEntityTypeConfiguration<VendingSlot>
{
    public void Configure(EntityTypeBuilder<VendingSlot> builder)
    {
        builder.ToTable("VendingSlots");

        builder.Property(vs => vs.SlotName).IsRequired().HasMaxLength(50);
        builder.Property(vs => vs.SlotDescription).HasMaxLength(250);
        builder.Property(vs => vs.SlotPrice).HasColumnType("decimal(18,2)");

        builder.HasOne(vs => vs.Vending)
               .WithMany()
               .HasForeignKey(vs => vs.VendingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vs => vs.Product)
               .WithMany(p => p.VendingSlots)
               .HasForeignKey(vs => vs.ProductId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasQueryFilter(filter => !filter.IsDeleted);
    }
}
