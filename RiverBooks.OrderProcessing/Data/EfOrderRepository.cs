using Microsoft.EntityFrameworkCore;

namespace RiverBooks.OrderProcessing.Data;

internal class EfOrderRepository(OrderProcessingDbContext dbContext) : IOrderRepository
{
  public async Task<List<Order>> ListAsync(CancellationToken cancellationToken = default)
  {
    return await dbContext.Orders.ToListAsync(cancellationToken);
  }

  public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
  {
    await dbContext.Orders.AddAsync(order, cancellationToken);
  }

  public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    await dbContext.SaveChangesAsync(cancellationToken);
  }
}
