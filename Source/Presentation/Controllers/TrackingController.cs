using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Constants;
using Tracking.Api.Core.Enums;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Models;
using Tracking.Api.Core.ResponseModels;
using Tracking.Api.RequestModels;

namespace Tracking.Api.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting(AppConstants.RateLimitPolicy)]
public class TrackingController(
    ITrackingJobPublisher publisher,
    ITrackingJobRepository repository,
    EnvironmentSettings settings) : ControllerBase
{
    private readonly ITrackingJobPublisher _publisher = publisher;
    private readonly ITrackingJobRepository _repository = repository;
    private readonly EnvironmentSettings.TrackingSettings _settings = settings.Tracking;

    /// <summary>
    /// Submit a new tracking request. Returns a job ID that can be used to check the status.
    /// </summary>
    /// <param name="request">Tracking number and carrier code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>202 Accepted with job ID and status URL</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TrackingJobResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitTrackingRequest(
        [FromBody] TrackingRequest request,
        CancellationToken cancellationToken)
    {
        var job = new TrackingJob
        {
            JobId = Guid.NewGuid(),
            TrackingNumber = request.TrackingNumber,
            Carrier = request.CarrierCode,
            Status = TrackingJobStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.SaveJobAsync(job, cancellationToken);
        await _publisher.PublishAsync(job, cancellationToken);

        var response = new TrackingJobResponse
        {
            JobId = job.JobId,
            Status = TrackingJobStatus.Pending,
            StatusUrl = Url.Action(nameof(GetTrackingStatus), new { jobId = job.JobId })!,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_settings.CacheExpirationMinutes)
        };

        return AcceptedAtAction(nameof(GetTrackingStatus), new { jobId = job.JobId }, response);
    }

    /// <summary>
    /// Get the status of a tracking job.
    /// </summary>
    /// <param name="jobId">The job ID returned from the submit endpoint</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>
    /// 200 OK if completed successfully,
    /// 202 Accepted if still processing,
    /// 410 Gone if expired or not found,
    /// 500 Internal Server Error if failed
    /// </returns>
    [HttpGet("{jobId}")]
    [ProducesResponseType(typeof(TrackingStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TrackingStatusResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status410Gone)]
    [ProducesResponseType(typeof(TrackingStatusResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTrackingStatus(
        [FromRoute] Guid jobId,
        CancellationToken cancellationToken)
    {
        var job = await _repository.GetJobAsync(jobId, cancellationToken);

        if (job is null)
        {
            throw new FriendlyException(
                "Tracking Job Not Found",
                StatusCodes.Status410Gone,
                "Tracking job has expired or does not exist. Results are only available for 5 minutes after submission.");
        }

        var response = new TrackingStatusResponse
        {
            JobId = job.JobId,
            Status = job.Status,
            Result = job.Result,
            ErrorMessage = job.ErrorMessage,
            CompletedAt = job.CompletedAt
        };

        return job.Status switch
        {
            TrackingJobStatus.Completed => Ok(response),
            TrackingJobStatus.Failed => throw new FriendlyException(
                "Tracking Job Failed",
                StatusCodes.Status500InternalServerError,
                job.ErrorMessage ?? "An error occurred while processing the tracking request."),
            TrackingJobStatus.Pending or TrackingJobStatus.Processing => AcceptedAtAction(nameof(GetTrackingStatus), new { jobId }, response),
            _ => throw new FriendlyException(
                "Unknown Job Status",
                StatusCodes.Status500InternalServerError,
                $"Job is in an unexpected state: {job.Status}")
        };
    }
}
