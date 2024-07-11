using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Interfaces;

namespace RiverBooks.OrderProcessing.Endpoints;

internal class ListOrdersForUserQueryHandler(IOrderRepository _orderRepository) :
  IRequestHandler<ListOrdersForUserQuery, Result<List<OrderSummary>>>
{
  public async Task<Result<List<OrderSummary>>> Handle(ListOrdersForUserQuery request, CancellationToken cancellationToken)
  {
    // look up UserId for EmailAddress

    // TODOs: Filter by User
    var orders = await _orderRepository.ListAsync(cancellationToken);

    var summaries = orders.Select(o => new OrderSummary()
    {
      DateCreated = o.DateCreated,
      OrderId = o.Id,
      UserId = o.UserId,
      Total = o.OrderItems.Sum(oi => oi.UnitPrice), // need to include orderItems
    }).ToList();

    return Result.Success(summaries);
  }
}
