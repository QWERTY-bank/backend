using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Quartz.Repayments.Configurations
{
    public class RepaymentsPlannerOptionsConfigure(IConfiguration configuration) : IConfigureOptions<RepaymentsPlannerOptions>
    {
        private readonly string valueKey = "Jobs:Repayments:Planner";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(RepaymentsPlannerOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
