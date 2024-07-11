using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.UserEndpoints;

internal class Create(UserManager<ApplicationUser> userManager) : Endpoint<CreateUserRequest>
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;

  public override void Configure()
  {
    Post("/users");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateUserRequest request, CancellationToken ct)
  {
    var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
    var result = await _userManager.CreateAsync(user, request.Password);

    if (result.Succeeded)
    {
      await SendOkAsync(ct);
    }
    else
    {
      await SendAsync(result.Errors.Select(e => e.Description), StatusCodes.Status400BadRequest, ct);
    }
  }
}
