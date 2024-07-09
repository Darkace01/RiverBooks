using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases.User;
internal record ListAddressesQuery(string EmailAddress) :
  IRequest<Result<List<UserAddressDto>>>;
