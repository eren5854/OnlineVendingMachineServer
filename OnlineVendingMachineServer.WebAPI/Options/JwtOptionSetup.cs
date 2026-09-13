using Microsoft.Extensions.Options;

namespace OnlineVendingMachineServer.WebAPI.Options;

public sealed class JwtOptionSetup(
    IConfiguration configuration) : IConfigureOptions<JwtOption>
{
    public void Configure(JwtOption options)
    {
        configuration.GetSection("Jwt").Bind(options);
    }
}