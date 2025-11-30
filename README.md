# Tracking API

A production-ready asynchronous package tracking API built with .NET 8, supporting multiple carriers with a scalable pub/sub architecture.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## Overview

The Tracking API transforms synchronous package tracking into a high-performance asynchronous system. Submit tracking requests and receive immediate responses while the system processes them in the background using a channel-based pub/sub pattern.

**Why Async?**
- **Fast responses**: Return immediately, don't wait for carrier APIs
- **Scalability**: Handle thousands of requests without blocking
- **Resilience**: Client requests succeed even if carrier APIs are slow
- **Better UX**: Clients poll for results at their convenience

---

## Architecture

### How It Works

```
1. Client submits tracking request → Returns Job ID immediately (202 Accepted)
2. Job published to in-memory channel
3. Background service picks up job and processes
4. Client polls for results using Job ID
5. Results cached for 5 minutes
```

**Key Components:**
- **Channel-based Queue**: Bounded channel (1000 capacity) for async job processing
- **Background Service**: Processes up to 10 jobs concurrently
- **Memory Cache**: Stores results with 5-minute TTL
- **Carrier Trackers**: Pluggable implementations (UPS, FedEx coming soon)

---

## Features

### Core Features
✅ Asynchronous processing with background jobs  
✅ UPS tracking support (FedEx coming soon)  
✅ Concurrent processing (10 jobs simultaneously)  
✅ Temporary result caching (5 minutes)  
✅ Rate limiting per API key (50/min, 990/day)  
✅ SOLID architecture with dependency injection  

### Production Ready
✅ Structured logging with Serilog  
✅ Health check endpoint  
✅ Swagger/OpenAPI documentation  
✅ Graceful shutdown handling  
✅ User-friendly error messages  
✅ Request correlation with job IDs  

---

## Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

```bash
# Clone and navigate
git clone https://github.com/yourusername/tracking-api.git
cd tracking-api

# Build and run
dotnet build
dotnet run --project Source/Tracking.Api.csproj

# Access Swagger UI
# Navigate to: https://localhost:5001/swagger
```

### Configuration

Edit `Source/appsettings.json`:

```json
{
  "AppSettings": {
    "XApiKey": "your-api-key-here",
    "ProxyUrl": "",
    "Tracking": {
      "ChannelCapacity": 1000,
      "CacheExpirationMinutes": 5,
      "MaxConcurrentProcessing": 10,
      "ProcessingDelayMs": 100
    }
  }
}
```

### Quick Test

```bash
# Submit tracking request
curl -X POST https://localhost:5001/tracking \
  -H "Content-Type: application/json" \
  -H "x-api-key: test" \
  -d '{
    "trackingNumber": "1Z999AA10123456784",
    "carrierCode": "UPS"
  }'

# Returns:
{
  "jobId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Pending",
  "statusUrl": "/tracking/3fa85f64-5717-4562-b3fc-2c963f66afa6"
}

# Check status (wait a few seconds)
curl https://localhost:5001/tracking/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "x-api-key: test"
```

---

## API Documentation

### Endpoints

#### `POST /tracking`

Submit a tracking request for asynchronous processing.

**Request:**
```json
{
  "trackingNumber": "1Z999AA10123456784",
  "carrierCode": "UPS"
}
```

**Response:** `202 Accepted`
```json
{
  "jobId": "guid",
  "status": "Pending",
  "statusUrl": "/tracking/{jobId}",
  "expiresAt": "2025-11-29T22:35:00.000Z"
}
```

#### `GET /tracking/{jobId}`

Check the status of a tracking job.

**Status Codes:**
- `202 Accepted` - Still processing
- `200 OK` - Completed with tracking info
- `410 Gone` - Job expired (>5 minutes)
- `500 Internal Server Error` - Processing failed

For complete API documentation, see [API Usage Guide](docs/api-usage-guide.md).

---

## Configuration Reference

| Setting | Default | Description |
|---------|---------|-------------|
| `ProxyUrl` | `""` | Proxy URL (optional) |
| `ChannelCapacity` | 1000 | Max jobs in queue |
| `CacheExpirationMinutes` | 5 | Result cache TTL |
| `MaxConcurrentProcessing` | 10 | Concurrent jobs |
| `ProcessingDelayMs` | 100 | Delay between jobs |
| `RateLimit` | 50 | Requests per minute per API key |
| `DailyLimit` | 990 | Daily requests per API key |

---

## SOLID Architecture

The codebase follows SOLID principles with a clean architecture:

- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: Extend by adding new `ITracker` implementations
- **Liskov Substitution**: All trackers/repositories are interchangeable
- **Interface Segregation**: Small, focused interfaces
- **Dependency Inversion**: Depend on abstractions, not implementations

**Key Interfaces:**
- `ITrackingJobPublisher` - Publish jobs to queue
- `ITrackingJobProcessor` - Process tracking logic
- `ITrackingJobRepository` - Store/retrieve jobs
- `ITracker` - Carrier-specific tracking implementations

---

## Project Structure

```
Tracking.Api/
├── Source/
│   ├── Core/                    # Business logic
│   │   ├── Interfaces/         # Service abstractions
│   │   ├── Services/           # Implementations
│   │   ├── Models/             # Domain models
│   │   └── Configuration/      # Settings
│   ├── Infrastructure/          # External concerns
│   │   ├── BackgroundServices/ # Async processing
│   │   └── Repositories/       # Data access
│   ├── Presentation/            # API layer
│   │   ├── Controllers/        # HTTP endpoints
│   │   └── Middlewares/        # HTTP pipeline
│   └── Program.cs              # Entry point
├── terraform/                   # AWS infrastructure
└── docs/                        # Documentation
```

---

## Technology Stack

- **.NET 8** - Modern C# framework
- **System.Threading.Channels** - High-performance pub/sub
- **IMemoryCache** - In-memory result caching
- **Serilog** - Structured JSON logging
- **Swagger/OpenAPI** - API documentation

---

## Deployment

### Docker Compose

```bash
docker-compose up --build -d
```

**Infrastructure:**
- Lightsail Container Service with sidecar mitmproxy
- ECR for container images
- Route53 for DNS
- SSL certificate management

---

## Monitoring

### Health Check

```bash
curl https://localhost:5001/healthcheck
```

### Structured Logging

All logs are JSON-formatted with correlation IDs:

```json
{
  "Timestamp": "2025-11-29T22:30:00.000Z",
  "Level": "Information",
  "Message": "Processing tracking job",
  "JobId": "3fa85f64-...",
  "TrackingNumber": "1Z999AA10...",
  "Carrier": "UPS"
}
```

---

## Future Enhancements

**Carriers**
- Other carriers

**Scalability**
- Redis for distributed caching
- RabbitMQ/Service Bus for persistent queue
- Request deduplication

**Features**
- Batch processing for multiple tracking numbers
- Circuit breaker with Polly

**Observability**
- OpenTelemetry distributed tracing
- Performance metrics dashboard

---


## License

This project is licensed under the MIT License.