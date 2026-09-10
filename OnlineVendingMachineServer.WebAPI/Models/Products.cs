namespace OnlineVendingMachineServer.WebAPI.Models;

public sealed class Products : Entity
{
    public string ProductName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; }
    public decimal ProductPrice { get; set; } = 0.00M;
    public string? ProductImage { get; set; }
    public short ProductQuantity { get; set; } = 0;
    public Guid VendingId { get; set; } = default!;
    public Vendings Vending { get; set; } = default!;
}
