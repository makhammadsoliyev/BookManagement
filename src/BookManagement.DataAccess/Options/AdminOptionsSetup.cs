using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BookManagement.DataAccess.Options;

public class AdminOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<AdminOptions>
{
    public void Configure(AdminOptions options)
    {
        configuration
            .GetRequiredSection(nameof(AdminOptions))
            .Bind(options);
    }
}
