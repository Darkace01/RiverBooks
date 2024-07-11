using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class EfIUserStreetAddressRepository(UsersDbContext _dbContext) : IReadOnlyUserStreetAddressRepository
{
  public async Task<UserStreetAddress?> GetById(Guid userStreetAddressId)
  {
    return await _dbContext.UserStreetAddresses.
        SingleOrDefaultAsync(a => a.Id == userStreetAddressId);
  }
}
