using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.ResponseModels;

public record TrackingJobResponse
{
    public required Guid JobId { get; init; }
    public required TrackingJobStatus Status { get; init; }
    public required string StatusUrl { get; init; }
    public required DateTime ExpiresAt { get; init; }
}
