namespace OnlineVendingMachineServer.WebAPI.Models;

public sealed class GoogleToken : Entity
{
    public string UserId { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public long? ExpiresInSeconds { get; set; }
    public DateTime IssuedUtc { get; set; }
}
