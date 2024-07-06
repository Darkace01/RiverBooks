
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books.Data;
internal class EfBookRepository(BookDbContext dbContext) : IBookRepository
{
  private readonly BookDbContext _dbContext = dbContext;

  public Task AddAsync(Book book, CancellationToken cancellationToken = default)
  {
    _dbContext.Add(book);
    return Task.CompletedTask;
  }

  public Task DeleteAsync(Book book, CancellationToken cancellationToken = default)
  {
    _dbContext.Remove(book);
    return Task.CompletedTask;
  }

  public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Books.FindAsync(id, cancellationToken);
  }

  public Task<List<Book>> ListAsync(CancellationToken cancellationToken = default)
  {
    return _dbContext.Books.ToListAsync(cancellationToken);
  }

  public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
