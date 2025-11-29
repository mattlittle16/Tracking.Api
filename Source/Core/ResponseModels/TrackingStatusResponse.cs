using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.ResponseModels;

public record TrackingStatusResponse
{
    public required Guid JobId { get; init; }
    public required TrackingJobStatus Status { get; init; }
    public TrackingInfo? Result { get; init; }
    public string? ErrorMessage { get; init; }
    public DateTime? CompletedAt { get; init; }
}
