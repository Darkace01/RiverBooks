namespace RiverBooks.OrderProcessing;

internal interface IOrderRepository
{
  Task<List<Order>> ListAsync(CancellationToken cancellationToken = default);
  Task AddAsync(Order order, CancellationToken cancellationToken = default);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
