namespace RiverBooks.Users;

public interface IApplicationUserRepository
{
  Task<ApplicationUser> GetUserWithCartByEmailAsync(string email, CancellationToken cancellationToken = default);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
