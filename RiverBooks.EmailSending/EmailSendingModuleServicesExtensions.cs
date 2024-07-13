using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServicesExtensions
{
  public static IServiceCollection AddEmailSendingModuleServices(this IServiceCollection services,
                                                                 ConfigurationManager config,
                                                                 ILogger logger,
                                                                 List<Assembly> mediatRAssemblies)
  {
    // configure MongoDb
    services.Configure<MongoDBSettings>(config.GetSection("MongoDB"));
    services.AddMongoDB(config);
    // Add module services here
    services.AddTransient<ISendEmail, MimeKitEmailSender>();
    services.AddTransient<IOutboxService, MongoDbOutboxService>();
    services.AddTransient<ISendEmailsFromOutboxService, DefaultSendEmailsFromOutboxService>();
    // If using MediatR in this module, add any assemblies that contain handlers to the list
    mediatRAssemblies.Add(typeof(EmailSendingModuleServicesExtensions).Assembly);

    // Add BackgroundWorker
    services.AddHostedService<EmailSendingBackgroundService>();
    logger.Information("{Module} module services registered", "Email Sending");
    return services;
  }

  public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
  {

    // Register the MongoDB client as a singleton
    services.AddSingleton<IMongoClient>(sp =>
    {
      var settings = configuration.GetSection("MongoDB").Get<MongoDBSettings>();
      return new MongoClient(settings!.ConnectionString);
    });

    // Register the MongoDB database as a singleton
    services.AddSingleton(sp =>
    {
      var settings = configuration.GetSection("MongoDB").Get<MongoDBSettings>();
      var client = sp.GetRequiredService<IMongoClient>();
      return client.GetDatabase(settings!.DatabaseName);
    });

    //// Optionally register specific collections here a scoped or singleton service
    /// Example for a 'EmailOutboxEnity' collection
    services.AddTransient(sp =>
    {
      var database = sp.GetService<IMongoDatabase>();
      return database!.GetCollection<EmailOutboxEntity>("EmailOutboxEntityCollection");
    });
    return services;
  }
}
