using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BookManagement.BusinessLogic.Options;

public class JwtOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<JwtOptions>
{
    public void Configure(JwtOptions options)
    {
        configuration.GetRequiredSection(nameof(JwtOptions)).Bind(options);
    }
}
