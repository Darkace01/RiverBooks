using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.User.GetById;

internal class GetUserByIdQueryHandler(IApplicationUserRepository _userRepository) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
  public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

    if (user is null) return Result.NotFound();

    return new UserDto(Guid.Parse(user.Id), user.Email!);
  }
}
