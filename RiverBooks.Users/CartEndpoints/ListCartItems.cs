using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Cart.ListItems;
namespace RiverBooks.Users.CartEndpoints;

internal class ListCartItems(IMediator _mediator) : EndpointWithoutRequest<CartResponse>
{
  public override void Configure()
  {
    Get("/cart");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var query = new ListCartItemsQuery(emailAddress!);

    var result = await _mediator.Send(query);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    await SendOkAsync(new CartResponse { CartItems = result.Value }, ct);
  }
}
