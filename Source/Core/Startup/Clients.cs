using System.Net;
using System.Security.Authentication;
using Tracking.Api.Core.Constants;

namespace Tracking.Api.Core.Startup;

public static class Clients
{
    public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient(AppConstants.UpsClientName, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestVersion = HttpVersion.Version11;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            UseCookies = true,
            AllowAutoRedirect = true,
            Proxy = new WebProxy(new Uri("http://mitm-proxy:8080")),
            UseProxy = true,
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

        return services;
    }
}