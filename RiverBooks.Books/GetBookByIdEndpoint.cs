using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace RiverBooks.Books;
internal class GetBookByIdEndpoint(IBookService bookService) :
    Endpoint<GetBookByIdRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;

  public override void Configure()
  {
    Get("/books/{Id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetBookByIdRequest request, CancellationToken ct)
  {
    var book = await _bookService.GetBookByIdAsync(request.Id, ct);

    if (book is null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await SendAsync(book, StatusCodes.Status200OK, ct);
  }
}
