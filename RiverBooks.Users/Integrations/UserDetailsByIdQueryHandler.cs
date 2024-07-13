using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.UseCases.User.GetById;

namespace RiverBooks.Users.Integrations;

internal class UserDetailsByIdQueryHandler(IMediator _mediator):
  IRequestHandler<UserDetailsByIdQuery, Result<UserDetails>>
{
  public async Task<Result<UserDetails>> Handle(UserDetailsByIdQuery request, CancellationToken cancellationToken)
  {
    var query = new GetUserByIdQuery(request.UserId);
    var result = await _mediator.Send(query, cancellationToken);

    return result.Map(user => new UserDetails(user.UserId, user.EmailAddress));
  }
}
