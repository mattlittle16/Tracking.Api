FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app 

COPY ["Tracking.Api.csproj", "./"]
COPY ["Core/", "Core/"]
COPY ["Infrastructure/", "Infrastructure/"]
COPY ["Presentation/", "Presentation/"]
COPY ["Program.cs", "./"]
COPY ["appsettings.json", "./"]

RUN dotnet restore "Tracking.Api.csproj"

# Build in Release mode for production optimization
RUN dotnet publish "Tracking.Api.csproj" -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime 
WORKDIR /app 

# Add non-root user for security
RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

COPY --from=build --chown=appuser:appuser /app/out ./ 

# Install curl for health checks (keep it lean)
RUN apt-get update && apt-get install -y --no-install-recommends curl && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Temporarily run as root to bind to port 80
# USER appuser

EXPOSE 80
ENV ASPNETCORE_URLS="http://*:80"

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=30s --retries=3 \
  CMD curl -f http://localhost/healthcheck || exit 1

ENTRYPOINT ["dotnet", "Tracking.Api.dll"]
