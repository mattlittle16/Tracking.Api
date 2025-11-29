using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Tracking.Api.Core.ResponseModels;

namespace Tracking.Api.Core.Configuration;

public class HostRateLimiterPolicy : IRateLimiterPolicy<string>
{
    private EnvironmentSettings _environmentSettings;

    public HostRateLimiterPolicy(IOptions<EnvironmentSettings> options)
    {
        _environmentSettings = options.Value;
    }

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.GetFixedWindowLimiter(httpContext.Request.Headers.Host.ToString(),
            partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = _environmentSettings.RateLimit,
                Window = TimeSpan.FromMinutes(_environmentSettings.RateLimitTimeInMinutes),
            });
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } =
    (context, _) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.ContentType = "application/json";   
        context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(new FriendlyExceptionResponse {  Title = "Too Many Requests", Status = 429, Details = "Too many requests" }));

        return new ValueTask();
    };
}