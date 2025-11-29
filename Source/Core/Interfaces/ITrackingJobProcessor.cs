using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Interfaces;

public interface ITrackingJobProcessor
{
    Task ProcessAsync(TrackingJob job, CancellationToken cancellationToken = default);
}
