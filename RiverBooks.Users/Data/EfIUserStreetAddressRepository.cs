using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal class EfIUserStreetAddressRepository(UsersDbContext _dbContext) : IReadOnlyUserStreetAddressRepository
{
  public async Task<UserStreetAddress?> GetById(Guid userStreetAddressId)
  {
    return await _dbContext.UserStreetAddresses.
        SingleOrDefaultAsync(a => a.Id == userStreetAddressId);
  }
}
