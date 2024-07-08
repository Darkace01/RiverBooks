using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Users.Data;
using Serilog;

namespace RiverBooks.Users;

public static class UserModuleServiceExtensions
{
  public static IServiceCollection AddUserModuleServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger,
    List<System.Reflection.Assembly> mediatRAssemblies)
  {
    string? connectionString = config.GetConnectionString("UsersConnectionString");
    services.AddDbContext<UsersDbContext>(options =>
      options.UseSqlServer(connectionString));

    services.AddIdentityCore<ApplicationUser>()
        .AddEntityFrameworkStores<UsersDbContext>();

    // Add User Serices
    services.AddScoped<IApplicationUserRepository, EfApplicationUserRepository>();

    // if using MediatR in this module, add any assemblies that contain handlers to the list
    mediatRAssemblies.Add(typeof(UserModuleServiceExtensions).Assembly);

    logger.Information("{Module} module services registered", "Users");
    return services;
  }
}
