using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Requests.Configurations
{
    public class CoreRequestOptionsConfigure(IConfiguration configuration) : IConfigureOptions<CoreRequestOptions>
    {
        private readonly string valueKey = "CoreRequest";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(CoreRequestOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
