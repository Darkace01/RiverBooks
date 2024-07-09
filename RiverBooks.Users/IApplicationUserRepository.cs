namespace RiverBooks.Users;

public interface IApplicationUserRepository
{
  Task<ApplicationUser> GetUserWithAddressByEmailAsync(string emailAddress, CancellationToken cancellationToken = default);
  Task<ApplicationUser> GetUserWithCartByEmailAsync(string emailAddress, CancellationToken cancellationToken = default);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
