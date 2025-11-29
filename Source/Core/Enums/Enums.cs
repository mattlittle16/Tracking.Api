namespace Tracking.Api.Core.Enums;

public enum CarrierCode
{
    UPS,
    FedEx
};

public enum TrackingStatus
{
    InTransit,
    Delivered,
    Exception,
    InfoReceived,
    OutForDelivery,
    Unknown
}