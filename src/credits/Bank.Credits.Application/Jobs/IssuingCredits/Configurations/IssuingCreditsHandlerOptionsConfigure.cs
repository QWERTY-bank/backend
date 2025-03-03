using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.IssuingCredits.Configurations
{
    public class IssuingCreditsHandlerOptionsConfigure(IConfiguration configuration) : IConfigureOptions<IssuingCreditsHandlerOptions>
    {
        private readonly string valueKey = "Jobs:IssuingCredits:Handler";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(IssuingCreditsHandlerOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
