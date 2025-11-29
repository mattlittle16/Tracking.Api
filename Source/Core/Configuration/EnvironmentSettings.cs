namespace Tracking.Api.Core.Configuration;

public sealed class EnvironmentSettings 
{
    public string XApiKey { get; set; } = string.Empty;

    public int RateLimit { get; set; } = 50;

    public int RateLimitTimeInMinutes { get; set; } = 1;

    public int DailyLimit { get; set; } = 990;

    public AppUrls Urls { get; set; } = new AppUrls();

    public TrackingSettings Tracking { get; set; } = new TrackingSettings();

    public sealed class AppUrls
    {
        public string UpsMain { get; set; } = string.Empty;
        public string UpsTrack { get; set; } = string.Empty;
    }

    public sealed class TrackingSettings
    {
        public int ChannelCapacity { get; set; } = 1000;
        public int CacheExpirationMinutes { get; set; } = 5;
        public int MaxConcurrentProcessing { get; set; } = 10;
        public int ProcessingDelayMs { get; set; } = 100;
    }
}