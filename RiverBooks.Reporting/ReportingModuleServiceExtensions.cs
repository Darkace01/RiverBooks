using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace RiverBooks.Reporting;

public static class ReportingModuleServiceExtensions
{
  public static IServiceCollection AddReportingModuleServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger,
    List<System.Reflection.Assembly> mediatRAssemblies)
  {
    // Add User Serices

    // if using MediatR in this module, add any assemblies that contain handlers to the list
    mediatRAssemblies.Add(typeof(ReportingModuleServiceExtensions).Assembly);

    logger.Information("{Module} module services registered", "Reporting");
    return services;
  }
}
