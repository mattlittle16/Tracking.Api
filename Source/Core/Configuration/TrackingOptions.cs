namespace Tracking.Api.Core.Configuration;

public class TrackingOptions
{
    public int ChannelCapacity { get; set; } = 1000;
    public int CacheExpirationMinutes { get; set; } = 5;
    public int MaxConcurrentProcessing { get; set; } = 10;
    public int ProcessingDelayMs { get; set; } = 100;
}
