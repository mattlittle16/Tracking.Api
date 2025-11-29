using System.Threading.Channels;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Services;

public class TrackingJobPublisher(Channel<TrackingJob> channel) : ITrackingJobPublisher
{
    private readonly ChannelWriter<TrackingJob> _writer = channel.Writer;

    public ValueTask PublishAsync(TrackingJob job, CancellationToken cancellationToken = default)
    {
        return _writer.WriteAsync(job, cancellationToken);
    }
}
