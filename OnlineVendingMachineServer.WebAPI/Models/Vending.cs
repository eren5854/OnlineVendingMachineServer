namespace OnlineVendingMachineServer.WebAPI.Models;

public sealed class Vending : Entity
{
    public string VendingName { get; set; } = string.Empty;
    public string? VendingLocation { get; set; }
    public string? VendingType { get; set; }
    public bool VendingStatus { get; set; } = false;
    public string? VendingDescription { get; set; }
    public string? VendingImage { get; set; }
    public string VendingSerialNumber { get; set; } = string.Empty;
    public short VendingSlotCount { get; set; } = 0;
}
