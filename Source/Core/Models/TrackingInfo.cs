using Tracking.Api.Core.Enums;

namespace Tracking.Api.Core.Models;

public record TrackingInfo
{
    public required string TrackingNumber { get; init; }
    public required CarrierCode Carrier { get; init; }
    public DateTime? DeliveryDate { get; init; }
    public required List<Events> EventList { get; init; }
    public record Events
    {
        public required DateTime Date { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }
    }
}