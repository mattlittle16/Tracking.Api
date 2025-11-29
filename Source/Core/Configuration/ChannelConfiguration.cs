using System.Threading.Channels;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Configuration;

public static class ChannelConfiguration
{
    public static Channel<TrackingJob> CreateTrackingJobChannel(int capacity = 1000)
    {
        return Channel.CreateBounded<TrackingJob>(
            new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleWriter = false,
                SingleReader = false
            });
    }
}
