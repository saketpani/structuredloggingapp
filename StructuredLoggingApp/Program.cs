using MinimalAPIApp.Endpoints;
using MinimalAPIApp.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Configure Serilog for structured logging 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseExceptionHandling();

app.MapGet("/", () => "Structured Logging Using Serilog!");
app.MapBookEndpoints();

await app.RunAsync();
