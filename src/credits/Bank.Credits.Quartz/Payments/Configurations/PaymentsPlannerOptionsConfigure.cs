using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Quartz.Payments.Configurations
{
    public class PaymentsPlannerOptionsConfigure(IConfiguration configuration) : IConfigureOptions<PaymentsPlannerOptions>
    {
        private readonly string valueKey = "Jobs:Payments:Planner";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(PaymentsPlannerOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
