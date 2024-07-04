
namespace RiverBooks.Books;

internal class BookService(IBookRepository bookRepository) : IBookService
{
  private readonly IBookRepository _bookRepository = bookRepository;

  public async Task CreateBookAsync(BookDto newBook, CancellationToken cancellationToken = default)
  {
    var book = new Book(newBook.Id, newBook.Title, newBook.Author, newBook.Price);

    await _bookRepository.AddAsync(book, cancellationToken);
    await _bookRepository.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
  {
    var bookToDelete = await _bookRepository.GetByIdAsync(id);

    if (bookToDelete is not null)
    {
      await _bookRepository.DeleteAsync(bookToDelete, cancellationToken);
      await _bookRepository.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task<BookDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

    //TODOs: handle not found case
    if (book is null)
      return null;

    return new BookDto(book!.Id, book.Title, book.Author, book.Price);
  }

  public async Task<List<BookDto>> ListBooksAsync(CancellationToken cancellationToken = default)
  {
    var books = (await _bookRepository.ListAsync(cancellationToken))
        .Select(b => new BookDto(b.Id, b.Title, b.Author, b.Price))
        .ToList();
    return books;
  }

  public async Task UpdateBookPriceAsync(Guid id, decimal newPrice, CancellationToken cancellationToken = default)
  {
    // validate the price
    var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

    // handle not found case
    book!.UpdatePrice(newPrice);
    await _bookRepository.SaveChangesAsync(cancellationToken);
  }
}
