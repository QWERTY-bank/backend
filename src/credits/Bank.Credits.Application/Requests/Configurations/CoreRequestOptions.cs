using Bank.Common.Resilience;

namespace Bank.Credits.Application.Requests.Configurations
{
    public class CoreRequestOptions
    {
        public string BaseUrl { get; set; } = null!;
        public string GetAccountBalance { get; set; } = null!;
        public string UnitAccountDepositTransfer { get; set; } = null!;
        public string UnitAccountWithdrawTransfer { get; set; } = null!;

        public ResilienceConfiguration Resilience { get; set; } = ResilienceConfiguration.Null;
    }
}
