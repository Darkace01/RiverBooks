using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderProcessing.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;
internal class CreateOrderCommandHandler(IOrderRepository _orderRepository,ILogger<CreateOrderCommandHandler> _logger) : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
  public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    var items = request.OrderItems.Select(item => new OrderItem(item.BookId,
                                                               item.Quantity,
                                                               item.UnitPrice,
                                                               item.Description)).ToList();

    var shippingAddressId = new Address("123 Main St","", "Anytown", "USA", "12345", "Home");
    var billingAddressId = shippingAddressId;

    var newOrder = Order.Factory.Create(request.UserId,
                                        shippingAddressId,
                                        billingAddressId,
                                        items);

    await _orderRepository.AddAsync(newOrder, cancellationToken);
    await _orderRepository.SaveChangesAsync(cancellationToken);

    _logger.LogInformation("Created order {OrderId} for user {UserId}", newOrder.Id, newOrder.UserId);

    return new OrderDetailsResponse(newOrder.Id);
  }
}
