using Tracking.Api.Core.Enums;

namespace Tracking.Api.RequestModels;

public record TrackingRequest(
    string TrackingNumber,
    CarrierCode CarrierCode
);