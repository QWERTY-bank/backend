namespace Bank.Common.Resilience;

public class ResilienceConfiguration
{
    public static ResilienceConfiguration Null = new()
    {
        RetryCount = 0,
        EventsBeforeBreak = 0,
        DurationOfBreak = TimeSpan.Zero,
        Timeout = TimeSpan.Zero
    };

    public int RetryCount { get; set; } = 3;
    public int EventsBeforeBreak { get; set; } = 10;
    public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromSeconds(15);
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
}