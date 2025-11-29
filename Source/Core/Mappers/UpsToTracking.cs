using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Mappers;

public static partial class Mappers
{
    public static TrackingInfo UpsToTrack(this UpsTrackResponse upsTrack)
    {
        var details = upsTrack.TrackDetails[0];

        var status = details.PackageStatus switch
        {
            "In Transit" => TrackingStatus.InTransit,
            "Delivered" => TrackingStatus.Delivered,
            "Exception" => TrackingStatus.Exception,
            "Info Received" => TrackingStatus.InfoReceived,
            "Out for Delivery" => TrackingStatus.OutForDelivery,
            _ => TrackingStatus.Unknown
        };

        var events = new List<TrackingInfo.Events>();
        foreach (var milestone in details.Milestones)
        {
            var locationData = ParseLocation(milestone.Location);
            events.Add(new TrackingInfo.Events
            {
                
                Description = milestone.Name,
                Location = locationData.location,
                Country = locationData.country,
                Date = ParseDate(milestone.Date, milestone.Time) 
            });
        }

        var trackingInfo = new TrackingInfo
        {
            TrackingNumber = details.RequestedTrackingNumber,
            Carrier = CarrierCode.UPS,
            Status = status,
            DeliveryDate = events.FirstOrDefault(e => e.Description.Contains("delivered", StringComparison.OrdinalIgnoreCase))?.Date,
            EventList = events.OrderBy(e => e.Date).ToList()
        };

        return trackingInfo;
    }

    private static DateTime? ParseDate(string date, string time)
    {
        if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
            return null;

        ReadOnlySpan<char> dateSpan = date.AsSpan();
        ReadOnlySpan<char> timeSpan = time.AsSpan();

        int firstSlash = dateSpan.IndexOf('/');
        if (firstSlash < 0) return null;
        
        int secondSlash = dateSpan.Slice(firstSlash + 1).IndexOf('/');
        if (secondSlash < 0) return null;
        secondSlash += firstSlash + 1;

        if (!int.TryParse(dateSpan.Slice(0, firstSlash), out int month)) return null;
        if (!int.TryParse(dateSpan.Slice(firstSlash + 1, secondSlash - firstSlash - 1), out int day)) return null;
        if (!int.TryParse(dateSpan.Slice(secondSlash + 1), out int year)) return null;

        int spaceIndex = timeSpan.IndexOf(' ');
        if (spaceIndex < 0) return null;

        ReadOnlySpan<char> timePart = timeSpan.Slice(0, spaceIndex);
        ReadOnlySpan<char> meridiem = timeSpan.Slice(spaceIndex + 1);

        int colonIndex = timePart.IndexOf(':');
        if (colonIndex < 0) return null;

        if (!int.TryParse(timePart.Slice(0, colonIndex), out int hour)) return null;
        if (!int.TryParse(timePart.Slice(colonIndex + 1), out int minute)) return null;

        if (meridiem.StartsWith("P", StringComparison.OrdinalIgnoreCase) && hour != 12)
            hour += 12;
        else if (meridiem.StartsWith("A", StringComparison.OrdinalIgnoreCase) && hour == 12)
            hour = 0;

        try
        {
            return new DateTime(year, month, day, hour, minute, 0);
        }
        catch
        {
            return null;
        }
    }

    private static (string country, string location) ParseLocation(string location)
    {
        if (string.IsNullOrEmpty(location))
            return (string.Empty, string.Empty);

        ReadOnlySpan<char> span = location.AsSpan();
        
        int lastCommaIndex = span.LastIndexOf(',');
        if (lastCommaIndex < 0)
            return (location, string.Empty);

        ReadOnlySpan<char> countryPart = span.Slice(lastCommaIndex + 1).Trim();
        ReadOnlySpan<char> locationPart = span.Slice(0, lastCommaIndex).Trim();

        return (countryPart.ToString(), locationPart.ToString());
    }
}