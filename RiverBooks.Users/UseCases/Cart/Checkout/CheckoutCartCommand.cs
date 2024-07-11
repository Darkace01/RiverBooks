using Ardalis.Result;
using MediatR;
using OrderProcessing.Contracts;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.Cart.Checkout;
public record CheckoutCartCommand(string EmailAddress,
                                    Guid ShippingAddressId,
                                    Guid BillingAddressId)
  : IRequest<Result<Guid>>;


internal class CheckoutCartCommandHandler(IApplicationUserRepository _userRepository, IMediator _mediator) : IRequestHandler<CheckoutCartCommand, Result<Guid>>
{
  public async Task<Result<Guid>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddress, cancellationToken);

    if (user is null)
    {
      return Result.Unauthorized();
    }

    var items = user.CartItems.Select(item => new OrderItemDetails(item.BookId,
                                                                   item.Quantity,
                                                                   item.UnitPrice,
                                                                   item.Description)).ToList();

    var createOrderCommand = new CreateOrderCommand(Guid.Parse(user.Id),
                                                    request.ShippingAddressId,
                                                    request.BillingAddressId,
                                                    items);

    // TODOs: Consider replacing with message-based approach for perf reasons
    var result = await _mediator.Send(createOrderCommand, cancellationToken);

    if(!result.IsSuccess)
    {
      //Change from a Result<OrderDetailsResponse> to a Result<Guid>
      return result.Map(x => x.OrderId);
    }

    user.ClearCart();
    await _userRepository.SaveChangesAsync(cancellationToken);

    return Result.Success(result.Value.OrderId);
  }
}
