using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User.ListAddresses;

namespace RiverBooks.Users.UserEndpoints;
internal class ListAddresses(IMediator _mediator): EndpointWithoutRequest<AddressListResponse>
{
  public override void Configure()
  {
    Get("/users/addresses");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");
    var query = new ListAddressesQuery(emailAddress!);
    var result = await _mediator.Send(query, ct);

    if(result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
    }

    var response = new AddressListResponse();
    response.Addresses = result.Value;
    await SendOkAsync(response, ct);
  }
}
