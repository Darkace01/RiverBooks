using Ardalis.Result;

namespace RiverBooks.OrderProcessing;

internal interface IOrderAddressCache
{
  Task<Result<OrderAddress>> GetByIdAsync(Guid id);
  Task<Result> StoreAsync(OrderAddress orderAddress);
}
