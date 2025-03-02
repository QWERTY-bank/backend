namespace Bank.Credits.Application.Jobs.Configurations
{
    public class IssuingCreditsJobOptions
    {
        public JobOptions Planner { get; set; }
        public JobOptions Handler { get; set; }
    }
}
