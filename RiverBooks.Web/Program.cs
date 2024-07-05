using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
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

builder.Services.AddFastEndpoints()
    .AddAuthenticationJwtBearer(options =>
    {
        options.SigningKey = builder.Configuration["Auth:JwtSecret"];
    })
    .AddAuthorization()
    .SwaggerDocument();

// Add Module Services
builder.Services.AddBookServices(builder.Configuration, logger);
builder.Services.AddUserModuleServices(builder.Configuration, logger);

var app = builder.Build();

app.UseAuthentication()
  .UseAuthorization();

app.UseFastEndpoints()
  .UseSwaggerGen();

await app.RunAsync();

#pragma warning disable S1118 // Utility classes should not have public constructors
public partial class Program { } // Needed for tests
#pragma warning restore S1118 // Utility classes should not have public constructors
