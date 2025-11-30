using System.Net;
using System.Security.Authentication;
using Microsoft.Extensions.Options;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Constants;

namespace Tracking.Api.Core.Startup;

public static class Clients
{
    public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient(AppConstants.UpsClientName, (sp, client) =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestVersion = HttpVersion.Version11;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
        }).ConfigurePrimaryHttpMessageHandler(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<EnvironmentSettings>>().Value;
            
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true,
                AllowAutoRedirect = true
            };

            // Only use proxy if ProxyUrl is configured
            if (!string.IsNullOrEmpty(settings.ProxyUrl))
            {
                handler.Proxy = new WebProxy(new Uri(settings.ProxyUrl));
                handler.UseProxy = true;
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            }

            return handler;
        });

        return services;
    }
}