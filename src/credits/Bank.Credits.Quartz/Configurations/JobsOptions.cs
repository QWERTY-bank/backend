namespace Bank.Credits.Quartz.Configurations
{
    public class JobsOptions
    {
        public CommonJobOptions Payments { get; set; } = null!;
        public CommonJobOptions Repayments { get; set; } = null!;
    }
}
