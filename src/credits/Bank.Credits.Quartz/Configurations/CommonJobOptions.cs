namespace Bank.Credits.Quartz.Configurations
{
    public class CommonJobOptions
    {
        public JobOptions Planner { get; set; }
        public JobOptions Handler { get; set; }
    }
}
