﻿using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Integrations;
internal class UserAddressDetailsByIdQueryHandler(IReadOnlyUserStreetAddressRepository _addressRepo) :
  IRequestHandler<UserAddressDetailsByIdQuery, Result<UserAddressDetails>>
{
  public async Task<Result<UserAddressDetails>> Handle(UserAddressDetailsByIdQuery request, CancellationToken cancellationToken)
  {
    var address = await _addressRepo.GetById(request.AddressId);

    if (address is null) return Result.NotFound();

    Guid userId = Guid.Parse(address.UserId);

    var details = new UserAddressDetails(userId,
                                         address.Id,
                                         address.StreetAddress.Street1,
                                         address.StreetAddress.Street2,
                                         address.StreetAddress.City,
                                         address.StreetAddress.State,
                                         address.StreetAddress.PostalCode,
                                         address.StreetAddress.Country);

    return details;
  }
}
