using Tracking.Api.Core.Enums;

namespace Tracking.Api.Core.Models;

public class TrackingJob
{
    public required Guid JobId { get; init; }
    public required string TrackingNumber { get; init; }
    public required CarrierCode Carrier { get; init; }
    public TrackingJobStatus Status { get; set; }
    public TrackingInfo? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; set; }
}
