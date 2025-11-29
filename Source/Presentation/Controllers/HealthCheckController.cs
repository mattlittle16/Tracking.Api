using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tracking.Api.Core.Constants;

namespace Tracking.Api.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting(AppConstants.RateLimitPolicy)]
public class HealthCheckController() : ControllerBase
{        
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {              
       return Ok();
    }    
}
