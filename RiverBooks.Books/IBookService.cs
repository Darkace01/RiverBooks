
namespace RiverBooks.Books;

internal interface IBookService
{
  Task<List<BookDto>> ListBooksAsync(CancellationToken cancellationToken = default);
  Task<BookDto> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default);
  Task CreateBookAsync(BookDto newBook, CancellationToken cancellationToken = default);
  Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default);
  Task UpdateBookPriceAsync(Guid id, decimal newPrice, CancellationToken cancellationToken = default);
}
