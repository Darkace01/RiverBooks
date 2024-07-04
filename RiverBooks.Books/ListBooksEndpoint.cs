using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace RiverBooks.Books;

internal class ListBooksEndpoint(IBookService bookService) :
    EndpointWithoutRequest<ListBooksResponse>
{
  private readonly IBookService _bookService = bookService;

  public override void Configure()
  {
    Get("/books");
    AllowAnonymous();
  }
  public override async Task HandleAsync(CancellationToken ct)
  {
    var books = await _bookService.ListBooksAsync(ct);

    await SendAsync(new ListBooksResponse { Books = books }, StatusCodes.Status200OK, ct);
  }
}
