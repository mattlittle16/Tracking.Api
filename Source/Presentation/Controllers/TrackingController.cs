using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tracking.Api.Core.Constants;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Services;
using Tracking.Api.RequestModels;

namespace Tracking.Api.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting(AppConstants.RateLimitPolicy)]
public class TrackingController([FromKeyedServices(AppConstants.UpsClientName)] ITracker upsTracker) : ControllerBase
{        
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] TrackingRequest request)
    {              
        var test = await upsTracker.TrackAsync(request.TrackingNumber);



       return Ok();
    }    
}
