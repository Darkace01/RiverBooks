using FastEndpoints;
using RiverBooks.Books;
using RiverBooks.Users;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
          config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();

// Add Module Services
builder.Services.AddBookServices(builder.Configuration, logger);
builder.Services.AddUserModuleServices(builder.Configuration, logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseFastEndpoints();

await app.RunAsync();

#pragma warning disable S1118 // Utility classes should not have public constructors
public partial class Program { } // Needed for tests
#pragma warning restore S1118 // Utility classes should not have public constructors
