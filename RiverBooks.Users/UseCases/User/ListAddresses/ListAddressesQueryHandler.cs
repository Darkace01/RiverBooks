using Ardalis.Result;
using MediatR;
using RiverBooks.Users.CartEndpoints;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.User.ListAddresses;

internal class ListAddressesQueryHandler(IApplicationUserRepository _userRepository) :
    IRequestHandler<ListAddressesQuery, Result<List<UserAddressDto>>>
{
  public async Task<Result<List<UserAddressDto>>> Handle(ListAddressesQuery request,
                                                          CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithAddressByEmailAsync(request.EmailAddress);
    if (user is null)
    {
      return Result.Unauthorized();
    }

    return user.Addresses.Select(a => new UserAddressDto(a.Id,
                                                                  a.StreetAddress.Street1,
                                                                  a.StreetAddress.Street2,
                                                                  a.StreetAddress.City,
                                                                  a.StreetAddress.State,
                                                                  a.StreetAddress.PostalCode,
                                                                  a.StreetAddress.Country))
                                  .ToList();
  }
}
