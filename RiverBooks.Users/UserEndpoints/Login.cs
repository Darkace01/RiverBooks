using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.UserEndpoints;

internal class Login(UserManager<ApplicationUser> userManager) : Endpoint<UserLoginRequest>
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;

  public override void Configure()
  {
    Post("/users/login");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UserLoginRequest request, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user is null)
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    var loginSuccessful = await _userManager.CheckPasswordAsync(user, request.Password);

    if (!loginSuccessful)
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    var jwtSecret = Config["Auth:JwtSecret"]!;
    var token = JwtBearer.CreateToken(p =>
    {
      p.SigningKey = jwtSecret;
      p.User.Claims.Add(new Claim("EmailAddress", user.Email!));
    });
    await SendOkAsync(token, ct);
  }
}
