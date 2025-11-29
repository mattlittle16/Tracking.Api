using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Interfaces;

public interface ITrackingJobRepository
{
    Task<TrackingJob?> GetJobAsync(Guid jobId, CancellationToken cancellationToken = default);
    Task SaveJobAsync(TrackingJob job, CancellationToken cancellationToken = default);
    Task UpdateJobStatusAsync(Guid jobId, TrackingJobStatus status, CancellationToken cancellationToken = default);
    Task CompleteJobAsync(Guid jobId, TrackingInfo result, CancellationToken cancellationToken = default);
    Task FailJobAsync(Guid jobId, string errorMessage, CancellationToken cancellationToken = default);
}
