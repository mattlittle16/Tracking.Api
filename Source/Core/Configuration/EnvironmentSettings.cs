namespace Tracking.Api.Core.Configuration;

public sealed class EnvironmentSettings 
{
    public string XApiKey { get; set; } = string.Empty;

    public int RateLimit { get; set; } = 50;

    public int RateLimitTimeInMinutes { get; set; } = 1;

    public int DailyLimit { get; set; } = 990;

    public AppUrls Urls { get; set; } = new AppUrls();

    public sealed class AppUrls
    {
        public string UpsMain { get; set; } = string.Empty;
        public string UpsTrack { get; set; } = string.Empty;
    }
}