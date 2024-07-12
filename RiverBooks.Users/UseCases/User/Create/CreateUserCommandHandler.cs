using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.UseCases.User.Create;

internal class CreateUserCommandHandler(UserManager<ApplicationUser> _userManager, IMediator _mediator) : IRequestHandler<CreateUserCommand, Result>
{
  public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {
    var user = new ApplicationUser
    {
      UserName = request.Email,
      Email = request.Email
    };
    var result = await _userManager.CreateAsync(user, request.Password);

    if (!result.Succeeded) return Result.Error(new ErrorList(result.Errors.Select(e => e.Description).ToArray()));

    // Send Welcome email
    var sendEmailCommand = new SendEmailCommand()
    {
      To = user.Email,
      From = "donotreply@test.com",
      Subject = "Welcome to RiverBooks",
      Body = "Welcome to RiverBooks! We are excited to have you as a member."
    };

    _ = await _mediator.Send(sendEmailCommand, cancellationToken);

    return Result.Success();
  }
}
