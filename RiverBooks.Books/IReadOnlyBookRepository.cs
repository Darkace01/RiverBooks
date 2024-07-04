namespace RiverBooks.Books;

internal interface IReadOnlyBookRepository
{
  Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  Task<List<Book>> ListAsync(CancellationToken cancellationToken = default);
}
