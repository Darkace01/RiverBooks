using System.Security.Claims;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Cart.AddItem;
namespace RiverBooks.Users.CartEndpoints;

internal class AddItem(IMediator _mediator) : Endpoint<AddCartItemRequest>
{
  public override void Configure()
  {
    Post("/cart");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(AddCartItemRequest req, CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var command = new AddItemToCartCommand(req.BookId, req.Quantity, emailAddress!);

    var result = await _mediator.Send(command, ct);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    if(result.Status == ResultStatus.Invalid)
    {
      await SendResultAsync(result.ToMinimalApiResult());
      return;
    }

    await SendOkAsync(ct);
  }
}
