using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Books.Integrations;

internal class BookDetailQueryHandler(IBookService bookService) :
  IRequestHandler<BookDetailsQuery, Result<BookDetailsResponse>>
{
  public async Task<Result<BookDetailsResponse>> Handle(BookDetailsQuery request, CancellationToken cancellationToken)
  {
    var book = await bookService.GetBookByIdAsync(request.BookId, cancellationToken);

    if (book is null)
    {
      return Result.NotFound();
    }

    var response = new BookDetailsResponse(book.Id, book.Title, book.Author, book.Price);

    return response;
  }
}
