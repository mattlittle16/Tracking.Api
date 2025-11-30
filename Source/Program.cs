using System.Net;
using System.Net.WebSockets;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Tracking.Api.Core.Configuration;
using Tracking.Api.Core.Constants;
using Tracking.Api.Core.Startup;
using Tracking.Api.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog with code-based configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .ReadFrom.Configuration(builder.Configuration) // Read log levels from appsettings
    .WriteTo.Console(new CompactJsonFormatter())
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("ApplicationName", "Weather.Api")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Tracking API",
        Version = "v1",
        Description = "Asynchronous package tracking API with support for multiple carriers. " +
                      "Submit tracking requests and poll for results using the returned job ID. " +
                      "Results are cached for 5 minutes."
    });

    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "x-api-key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "API Key needed to access the endpoints. X-API-KEY: {API Key}",
    });
    
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.Configure<EnvironmentSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(AppConstants.CORSPolicy,
        builder =>
        {
            builder.SetIsOriginAllowed(_ => true);
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowCredentials();
        });
});

var appSettings = builder.Configuration.GetSection("AppSettings");
var envSettings = new EnvironmentSettings();
appSettings.Bind(envSettings);
builder.Services.AddSingleton(envSettings);


builder.Services.RegisterHttpClients();
builder.Services.RegisterServices();


var app = builder.Build();
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath}{QueryString} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("QueryString", httpContext.Request.QueryString.Value);
        if (httpContext.Request.Query.Count > 0)
        {
            diagnosticContext.Set("QueryParameters", httpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString()));
        }
    };
});

//middleware
app.UseCors(AppConstants.CORSPolicy);
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<DailyRequestLimitMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();