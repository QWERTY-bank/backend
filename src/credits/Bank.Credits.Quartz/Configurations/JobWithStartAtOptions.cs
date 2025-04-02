namespace Bank.Credits.Quartz.Configurations
{
    public class JobWithStartAtOptions  : JobOptions
    {
        public DateTimeOffset StartAt { get; set; }
    }
}
