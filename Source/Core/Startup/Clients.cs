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
            client.Timeout = TimeSpan.FromSeconds(10);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            UseCookies = true,
            AllowAutoRedirect = true,
            //Proxy = new WebProxy(new Uri("http://127.0.0.1:8866")), // Fiddler's default port
            //UseProxy = true
        });

        return services;
    }
}