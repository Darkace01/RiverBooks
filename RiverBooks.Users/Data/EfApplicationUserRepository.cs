using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal class EfApplicationUserRepository(UsersDbContext _dbContext) : IApplicationUserRepository
{
  public async Task<ApplicationUser> GetUserWithCartByEmailAsync(string email, CancellationToken cancellationToken = default)
  {
    return await _dbContext.ApplicationUsers
                           .Include(u => u.CartItems)
                           .SingleAsync(u => u.Email == email, cancellationToken);
  }

  public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
