# Enterprise Structured Logging Application (.NET Core)

A production-ready observability blueprint demonstrating modern application logging and diagnostic telemetry practices in .NET Core using **Serilog**. This repository provides an architectural template for centralized log aggregation, high-performance tracing, and queryable diagnostic telemetry.

## Architectural & Observability Patterns
Instead of outputting fragile, flat text streams that require complex regular expressions to parse, this application structures logs as fully queryable JSON object payloads. It enforces the following production-grade patterns:

- **Semantic & Structured Logging:** Captures rich contextual metadata using property serialization instead of string interpolation, allowing log analytics tools to index and query fields instantly.
- **Log Enrichment:** Enforces automated context injection to automatically append operational variables—such as `ApplicationName`, `Environment`, `ThreadId`, `MachineName`, and structural request details—to every log boundary.
- **Sink Diversification:** Configured to dynamically route log streams simultaneously to multiple output sinks (e.g., colorized Console streams and automated daily rolling local Files) without altering core application code.
- **Performance Optimization (Asynchronous Sinking):** Features non-blocking asynchronous logging configurations (`Serilog.Sinks.Async`) to ensure high-throughput application performance isn't throttled by diagnostics or disk write I/O.
- **Dynamic Filtering & Levels:** Implements systematic control over log verbosity (`Verbose`, `Debug`, `Information`, `Warning`, `Error`, `Fatal`) to handle cost and data ingestion volume cleanly across staging and production environments.

## Tech Stack & Extensions
- **Runtime Framework:** .NET Core SDK
- **Core Engine:** Serilog
- **Configuration Integration:** Microsoft.Extensions.Configuration (Enabling zero-code logging adjustments via JSON settings files)
- **Ecosystem Sinks:** - `Serilog.Sinks.Console`
  - `Serilog.Sinks.File`
  - `Serilog.Sinks.Async`

## Configuration Architecture
The application leverages standard .NET configuration providers, abstracting sink behaviors entirely into `appsettings.json`. This allows devops and cloud engineers to modify log targets, override namespaces, and scale verbosity levels seamlessly during environment deployments without requiring re-compilation.

## How to Run & Verify

### Prerequisites
- .NET SDK (v8.0 or applicable version)

### Local Execution Steps
1. Clone the repository:
   ```bash
   git clone [https://github.com/saketpani/structuredloggingapp.git](https://github.com/saketpani/structuredloggingapp.git)
   cd structuredloggingapp
