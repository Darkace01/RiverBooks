using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Cart.Checkout;

namespace RiverBooks.Users.CartEndpoints;
public record CheckoutRequest(Guid ShippingAddressId, Guid BillingAddressId) : IRequest<CheckoutResponse>;
public record CheckoutResponse(Guid NewOrderId);
internal class Checkout(IMediator mediator) : Endpoint<CheckoutRequest, CheckoutResponse>
{

  public override void Configure()
  {
    Post("/cart/checkout");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CheckoutRequest request,
    CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var command = new CheckoutCartCommand(emailAddress!,
                                      request.ShippingAddressId,
                                      request.BillingAddressId);

    var result = await mediator.Send(command, ct);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
    }

    await SendOkAsync(new CheckoutResponse(result.Value));
  }
}
