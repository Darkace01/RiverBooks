namespace RiverBooks.Books;

internal interface IBookRepository : IReadOnlyBookRepository
{
  Task AddAsync(Book book, CancellationToken cancellationToken = default);
  Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
  Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
