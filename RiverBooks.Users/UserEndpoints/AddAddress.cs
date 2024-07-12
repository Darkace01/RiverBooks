using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User.AddAddress;

namespace RiverBooks.Users.UserEndpoints;
internal sealed class AddAddress(IMediator _mediator) : Endpoint<AddAddressRequest>
{
  public override void Configure()
  {
    Post("/users/addresses");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(AddAddressRequest request, CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");
    var command = new AddAddressToUserCommand(emailAddress!,
                                              request.Street1,
                                              request.Street2,
                                              request.City,
                                              request.State,
                                              request.PostalCode,
                                              request.Country);
    var result = await _mediator.Send(command, ct);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
    }
    await SendOkAsync(ct);
  }
}
