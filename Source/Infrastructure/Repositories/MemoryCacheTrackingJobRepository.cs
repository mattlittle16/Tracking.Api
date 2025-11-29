using Microsoft.Extensions.Caching.Memory;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Infrastructure.Repositories;

public class MemoryCacheTrackingJobRepository(
    IMemoryCache cache,
    EnvironmentSettings settings) : ITrackingJobRepository
{
    private readonly IMemoryCache _cache = cache;
    private readonly EnvironmentSettings.TrackingSettings _settings = settings.Tracking;

    public Task<TrackingJob?> GetJobAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        _cache.TryGetValue(GetCacheKey(jobId), out TrackingJob? job);
        return Task.FromResult(job);
    }

    public Task SaveJobAsync(TrackingJob job, CancellationToken cancellationToken = default)
    {
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(_settings.CacheExpirationMinutes))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.CacheExpirationMinutes * 2));

        _cache.Set(GetCacheKey(job.JobId), job, cacheOptions);
        return Task.CompletedTask;
    }

    public async Task UpdateJobStatusAsync(Guid jobId, TrackingJobStatus status, CancellationToken cancellationToken = default)
    {
        var job = await GetJobAsync(jobId, cancellationToken);
        if (job is null) return;

        job.Status = status;
        await SaveJobAsync(job, cancellationToken);
    }

    public async Task CompleteJobAsync(Guid jobId, TrackingInfo result, CancellationToken cancellationToken = default)
    {
        var job = await GetJobAsync(jobId, cancellationToken);
        if (job is null) return;

        job.Status = TrackingJobStatus.Completed;
        job.Result = result;
        job.CompletedAt = DateTime.UtcNow;
        await SaveJobAsync(job, cancellationToken);
    }

    public async Task FailJobAsync(Guid jobId, string errorMessage, CancellationToken cancellationToken = default)
    {
        var job = await GetJobAsync(jobId, cancellationToken);
        if (job is null) return;

        job.Status = TrackingJobStatus.Failed;
        job.ErrorMessage = errorMessage;
        job.CompletedAt = DateTime.UtcNow;
        await SaveJobAsync(job, cancellationToken);
    }

    private static string GetCacheKey(Guid jobId) => $"tracking_job_{jobId}";
}
