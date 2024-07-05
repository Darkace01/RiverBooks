using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : IdentityDbContext(options)
{
  public DbSet<ApplicationUser> ApplicationUsers { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.HasDefaultSchema("Users");
    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(builder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
       .HavePrecision(18, 6);
  }
}
