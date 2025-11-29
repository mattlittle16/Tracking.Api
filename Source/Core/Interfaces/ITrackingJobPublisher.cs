using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Interfaces;

public interface ITrackingJobPublisher
{
    ValueTask PublishAsync(TrackingJob job, CancellationToken cancellationToken = default);
}
