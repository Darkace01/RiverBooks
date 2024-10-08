﻿using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Interfaces;

public interface IApplicationUserRepository
{
  Task<ApplicationUser> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
  Task<ApplicationUser> GetUserWithAddressByEmailAsync(string emailAddress, CancellationToken cancellationToken = default);
  Task<ApplicationUser> GetUserWithCartByEmailAsync(string emailAddress, CancellationToken cancellationToken = default);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
