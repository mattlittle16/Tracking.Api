using System.Threading.Channels;
using System.Threading.RateLimiting;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Constants;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Models;
using Tracking.Api.Core.Services;
using Tracking.Api.Infrastructure.BackgroundServices;
using Tracking.Api.Infrastructure.Repositories;

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

        // Tracking channel (singleton)
        services.AddSingleton(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<EnvironmentSettings>();
            return ChannelConfiguration.CreateTrackingJobChannel(settings.Tracking.ChannelCapacity);
        });

        // Tracking services
        services.AddSingleton<ITrackingJobPublisher, TrackingJobPublisher>();
        services.AddScoped<ITrackingJobRepository, MemoryCacheTrackingJobRepository>();
        services.AddTransient<ITrackingJobProcessor, TrackingJobProcessor>();

        // Background service
        services.AddHostedService<TrackingBackgroundService>();

        return services;
    }
}