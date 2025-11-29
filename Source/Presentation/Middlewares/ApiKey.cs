using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Models;

namespace Tracking.Api.Presentation.Middlewares;

public class ApiKeyMiddleware(RequestDelegate next, IOptions<EnvironmentSettings> configuration)
{
    private const string APIKEYNAME = "x-api-key";

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger")
        || context.Request.Path.StartsWithSegments("/healthcheck"))
        {
            await next(context);
            return;
        }

        var apiKey = configuration.Value.XApiKey;

        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            throw new FriendlyException("Forbidden", 401, "API Key was not provided.");
        }

        if (!IsValidApiKey(apiKey, extractedApiKey!))
        {
            throw new FriendlyException("Forbidden", 401, "API Key is not valid.");
        }

        await next(context);
    }

    private static bool IsValidApiKey(string expectedKey, string providedKey)
    {
        if (string.IsNullOrEmpty(expectedKey) || string.IsNullOrEmpty(providedKey))
            return false;

        var expectedBytes = Encoding.UTF8.GetBytes(expectedKey);
        var providedBytes = Encoding.UTF8.GetBytes(providedKey);

        return CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }
}