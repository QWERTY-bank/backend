using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Quartz.Repayments.Configurations
{
    public class RepaymentsHandlerOptionsConfigure(IConfiguration configuration) : IConfigureOptions<RepaymentsHandlerOptions>
    {
        private readonly string valueKey = "Jobs:Repayments:Handler";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(RepaymentsHandlerOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
