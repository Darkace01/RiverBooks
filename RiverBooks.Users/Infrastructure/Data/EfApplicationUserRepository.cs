using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data;

internal class EfApplicationUserRepository(UsersDbContext _dbContext) : IApplicationUserRepository
{
  public async Task<ApplicationUser> GetUserWithAddressByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
  {
    return await _dbContext.ApplicationUsers
                          .Include(u => u.Addresses)
                          .SingleAsync(u => u.Email == emailAddress, cancellationToken);
  }

  public async Task<ApplicationUser> GetUserWithCartByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
  {
    return await _dbContext.ApplicationUsers
                           .Include(u => u.CartItems)
                           .SingleAsync(u => u.Email == emailAddress, cancellationToken);
  }

  public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
