using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Constants;
using Tracking.Api.Core.Interfaces;
using Tracking.Api.Core.Models;
using Tracking.Api.Core.Utils;
namespace Tracking.Api.Core.Services;

public class UpsTracker(IHttpClientFactory httpClientFactory, IOptions<EnvironmentSettings> options) : ITracker
{
    public async Task<TrackingInfo> TrackAsync(string trackingNumber, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(AppConstants.UpsClientName);
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
        client.DefaultRequestHeaders.Add("Accept", "*/*");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, zstd");
        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
        client.DefaultRequestHeaders.Add("Origin", "https://www.ups.com");

        var response  = await client.GetAsync(options.Value.Urls.UpsMain, cancellationToken);
        
        var cookies = response.Headers.GetValues("Set-Cookie");
        var token = string.Empty;
        foreach(var cookie in cookies)
        {
            if (cookie.StartsWith("X-XSRF-TOKEN-ST", StringComparison.OrdinalIgnoreCase))
            {
                token = AppStringExtensions.ExtractCookieValue(cookie, "X-XSRF-TOKEN-ST");
                break;
            }
        }

        var sb = new StringBuilder();
        sb.Append("{\"Locale\": \"en_US\",\"TrackingNumber\": [");
        sb.Append($"\"{trackingNumber}\"");
        sb.Append("],\"isBarcodeScanned\": false,\"Requester\": \"st\",\"ClientUrl\": \"https://www.ups.com/track?loc=en_US&requester=ST/\"}");

        client.DefaultRequestHeaders.Clear();  
        client.DefaultRequestHeaders.Add("User-Agent", "HTTPie/3.2.4");
        client.DefaultRequestHeaders.Add("Accept", "application/json, */*;q=0.5");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, zstd");
        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
        client.DefaultRequestHeaders.Add("Origin", "https://www.ups.com");
        client.DefaultRequestHeaders.Add("X-XSRF-TOKEN", token);

        var content = new StringContent(sb.ToString(), null, "application/json");
        var trackResponse = await client.PostAsync(options.Value.Urls.UpsTrack, content, cancellationToken);
        
        var trackResponseContent = await trackResponse.Content.ReadAsStringAsync(cancellationToken);



        return default!;
    }

    
}