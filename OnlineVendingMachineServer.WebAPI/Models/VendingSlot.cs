namespace OnlineVendingMachineServer.WebAPI.Models;

public sealed class VendingSlot : Entity
{
    public string SlotName { get; set; } = string.Empty;
    public string? SlotDescription { get; set; }
    public decimal SlotPrice { get; set; } = 0.00M;
    public short SlotQuantity { get; set; } = 0;
    public Guid VendingId { get; set; } = default!;
    public Vending Vending { get; set; } = default!;

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
}
