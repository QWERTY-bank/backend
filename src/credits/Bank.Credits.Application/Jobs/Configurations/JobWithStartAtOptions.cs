namespace Bank.Credits.Application.Jobs.Configurations
{
    public class JobWithStartAtOptions  : JobOptions
    {
        public DateTimeOffset StartAt { get; set; }
    }
}
