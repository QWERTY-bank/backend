namespace Bank.Common.OpenTelemetry
{
    internal class OpenTelemetryOptions
    {
        public string TracingGrpcEndpoint { get; set; } = null!;
        public string LoggingGrpcEndpoint { get; set; } = null!;
    }
}
