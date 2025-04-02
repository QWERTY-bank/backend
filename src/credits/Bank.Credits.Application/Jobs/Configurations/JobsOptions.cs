namespace Bank.Credits.Application.Jobs.Configurations
{
    public class JobsOptions
    {
        public CommonJobOptions Payments { get; set; } = null!;
        public CommonJobOptions Repayments { get; set; } = null!;
    }
}
