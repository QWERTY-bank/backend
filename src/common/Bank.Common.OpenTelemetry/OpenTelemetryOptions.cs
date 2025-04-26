namespace Bank.Common.OpenTelemetry
{
    internal class OpenTelemetryOptions
    {
        public string TracingGrpcEndpoint { get; set; } = null!; // = "http://tempo:4317";
    }
}
