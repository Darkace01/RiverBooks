using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.User.AddAddress;

internal class AddAddressToUserCommandHandler(IApplicationUserRepository _userRepository, ILogger<AddAddressToUserCommandHandler> _logger) : IRequestHandler<AddAddressToUserCommand, Result>
{
  public async Task<Result> Handle(AddAddressToUserCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithAddressByEmailAsync(request.EmailAddress, cancellationToken);

    if (user is null)
    {
      return Result.Unauthorized();
    }

    var addressToAdd = new Address(request.Street1,
                                   request.Street2,
                                   request.City,
                                   request.State,
                                   request.PostalCode,
                                   request.Country);

    var userAddress = user.AddAdress(addressToAdd);
    await _userRepository.SaveChangesAsync(cancellationToken);

    _logger.LogInformation("[UseCase] Added address {StreetAddress} to user {Email} (Total: {TotalAddresses})", userAddress.StreetAddress, user.Email, user.Addresses.Count);
    return Result.Success();
  }
}
