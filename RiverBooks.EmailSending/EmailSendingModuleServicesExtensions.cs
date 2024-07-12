using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServicesExtensions
{
  public static IServiceCollection AddEmailSendingModuleServices(this IServiceCollection services,
                                                                 ConfigurationManager config,
                                                                 ILogger logger,
                                                                 List<Assembly> mediatRAssemblies)
  {
    // Add module services here
    services.AddTransient<ISendEmail, MimeKitEmailSender>();
    // If using MediatR in this module, add any assemblies that contain handlers to the list
    mediatRAssemblies.Add(typeof(EmailSendingModuleServicesExtensions).Assembly);
    logger.Information("{Module} module services registered", "Email Sending");
    return services;
  }
}
