using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bank.Credits.Application.Jobs.IssuingCredits.Configurations
{
    public class IssuingCreditsPlannerOptionsConfigure(IConfiguration configuration) : IConfigureOptions<IssuingCreditsPlannerOptions>
    {
        private readonly string valueKey = "Jobs:IssuingCredits:Planner";
        private readonly IConfiguration _configuration = configuration;

        public void Configure(IssuingCreditsPlannerOptions options) => _configuration.GetSection(valueKey).Bind(options);
    }
}
