using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

public class OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options, IDomainEventDispatcher _dispatcher) : DbContext(options)
{
  internal DbSet<Order> Orders { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("OrderProcessing");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(modelBuilder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
       .HavePrecision(18, 6);
  }

  /// <summary>
  /// This is needed for domain events to work
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher is null) return result;

    var entitiesWithEvents = ChangeTracker.Entries<IHaveDomainEvents>()
            .Select(e => e.Entity)
    .Where(e => e.DomainEvents.Any())
            .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }
}
