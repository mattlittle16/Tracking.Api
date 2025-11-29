using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Core.Services;

public class TrackingJobProcessor(IServiceProvider serviceProvider) : ITrackingJobProcessor
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task ProcessAsync(TrackingJob job, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ITrackingJobRepository>();

        try
        {
            await repository.UpdateJobStatusAsync(job.JobId, TrackingJobStatus.Processing, cancellationToken);

            var tracker = GetTrackerForCarrier(scope.ServiceProvider, job.Carrier);
            var trackingInfo = await tracker.TrackAsync(job.TrackingNumber, cancellationToken);

            await repository.CompleteJobAsync(job.JobId, trackingInfo, cancellationToken);
        }
        catch (Exception ex)
        {
            await repository.FailJobAsync(job.JobId, ex.Message, cancellationToken);
        }
    }

    private static ITracker GetTrackerForCarrier(IServiceProvider serviceProvider, CarrierCode carrier)
    {
        return carrier switch
        {
            CarrierCode.UPS => serviceProvider.GetRequiredKeyedService<ITracker>(Core.Constants.AppConstants.UpsClientName),
            CarrierCode.FedEx => throw new NotImplementedException("FedEx tracking not yet implemented"),
            _ => throw new ArgumentException($"Unsupported carrier: {carrier}", nameof(carrier))
        };
    }
}
