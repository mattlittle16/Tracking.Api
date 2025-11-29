using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Presentation.Middlewares;

public class DailyRequestLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly EnvironmentSettings _settings;

    public DailyRequestLimitMiddleware(IMemoryCache cache, RequestDelegate next, IOptions<EnvironmentSettings> options)
    {
        _cache = cache;
        _next = next;
        _settings = options.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var cacheKey = DateTimeOffset.UtcNow.Date.ToString("MM-dd-yyyy")+"hits";
        if (_cache.TryGetValue(cacheKey, out int hits))
        {
            hits++;
            _cache.Set(cacheKey, hits, TimeSpan.FromHours(24));
        }
        else 
        {
            hits = 1;
            _cache.Set(cacheKey, hits, TimeSpan.FromHours(24));
        }

        if (hits == _settings.DailyLimit)
        {
            throw new FriendlyException("Too Many Requests", 429, "Daily request limit hit");
        }
        else
        {
            await _next(context);
        }        
    }
}