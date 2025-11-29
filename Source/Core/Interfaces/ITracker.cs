using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Interfaces;

public interface ITracker
{
    Task<TrackingInfo> TrackAsync(string trackingNumber, CancellationToken cancellationToken = default);
}