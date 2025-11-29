using System.Threading.Channels;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Infrastructure.BackgroundServices;

public class TrackingBackgroundService(
    Channel<TrackingJob> channel,
    ITrackingJobProcessor processor,
    EnvironmentSettings settings,
    ILogger<TrackingBackgroundService> logger) : BackgroundService
{
    private readonly ChannelReader<TrackingJob> _reader = channel.Reader;
    private readonly ITrackingJobProcessor _processor = processor;
    private readonly EnvironmentSettings.TrackingSettings _settings = settings.Tracking;
    private readonly ILogger<TrackingBackgroundService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Tracking Background Service started. Max concurrent processing: {MaxConcurrent}", 
            _settings.MaxConcurrentProcessing);

        var processingTasks = new List<Task>();

        try
        {
            await foreach (var job in _reader.ReadAllAsync(stoppingToken))
            {
                processingTasks.RemoveAll(t => t.IsCompleted);

                if (processingTasks.Count >= _settings.MaxConcurrentProcessing)
                {
                    var completedTask = await Task.WhenAny(processingTasks);
                    processingTasks.Remove(completedTask);
                }

                var processingTask = ProcessJobAsync(job, stoppingToken);
                processingTasks.Add(processingTask);
            }
        }
        finally
        {
            _logger.LogInformation("Tracking Background Service stopping. Waiting for {TaskCount} jobs to complete", 
                processingTasks.Count);
            await Task.WhenAll(processingTasks);
            _logger.LogInformation("Tracking Background Service stopped");
        }
    }

    private async Task ProcessJobAsync(TrackingJob job, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing tracking job {JobId} for tracking number {TrackingNumber} with carrier {Carrier}", 
                job.JobId, job.TrackingNumber, job.Carrier);

            if (_settings.ProcessingDelayMs > 0)
            {
                await Task.Delay(_settings.ProcessingDelayMs, cancellationToken);
            }

            await _processor.ProcessAsync(job, cancellationToken);

            _logger.LogInformation("Successfully completed tracking job {JobId}", job.JobId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing tracking job {JobId}: {ErrorMessage}", 
                job.JobId, ex.Message);
        }
    }
}
