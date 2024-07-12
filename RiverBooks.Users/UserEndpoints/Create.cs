using Ardalis.Result.AspNetCore;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User.Create;

namespace RiverBooks.Users.UserEndpoints;

internal class Create(IMediator _mediator) : Endpoint<CreateUserRequest>
{
  public override void Configure()
  {
    Post("/users");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateUserRequest request, CancellationToken ct)
  {
    var command = new CreateUserCommand(request.Email, request.Password);
    var result = await _mediator.Send(command, ct);

    if (!result.IsSuccess)
    {
      await SendResultAsync(result.ToMinimalApiResult());
      return;
    }
    await SendOkAsync(ct);
  }
}
