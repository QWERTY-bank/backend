using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.Payments.Configurations
{
    public class PaymentsHandlerOptionsConfigure(IConfiguration configuration) : IConfigureOptions<PaymentsHandlerOptions>
    {
        private readonly string valueKey = "Jobs:Payments:Handler";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(PaymentsHandlerOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
