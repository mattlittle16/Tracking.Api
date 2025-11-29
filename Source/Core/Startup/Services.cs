using System.Threading.RateLimiting;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Constants;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Services;

namespace Tracking.Api.Core.Startup;

public static class Services
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //caching and rate limiting
        services.AddMemoryCache();
        services.AddRateLimiter(options => {
            options.AddPolicy<string, HostRateLimiterPolicy>(AppConstants.RateLimitPolicy);            
        });        
        
        services.AddKeyedScoped<ITracker, UpsTracker>(AppConstants.UpsClientName);

        return services;
    }
}