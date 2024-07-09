using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderProcessing.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;
internal class CreateOrderCommandHandler(IOrderRepository _orderRepository,ILogger<CreateOrderCommandHandler> _logger, IOrderAddressCache _addressCache) : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
  public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    var items = request.OrderItems.Select(item => new OrderItem(item.BookId,
                                                               item.Quantity,
                                                               item.UnitPrice,
                                                               item.Description)).ToList();

    
    var shippingAddress = await _addressCache.GetByIdAsync(request.ShippingAddressId);
    var billingAddress = await _addressCache.GetByIdAsync(request.BillingAddressId);

    var newOrder = Order.Factory.Create(request.UserId,
                                        shippingAddress.Value.Address,
                                        billingAddress.Value.Address,
                                        items);

    await _orderRepository.AddAsync(newOrder, cancellationToken);
    await _orderRepository.SaveChangesAsync(cancellationToken);

    _logger.LogInformation("Created order {OrderId} for user {UserId}", newOrder.Id, newOrder.UserId);

    return new OrderDetailsResponse(newOrder.Id);
  }
}
