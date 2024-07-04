using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace RiverBooks.Books;

internal class UpdateBookPriceEndpoint(IBookService bookService) :
    Endpoint<UpdateBookPriceRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;

  public override void Configure()
  {
    Post("/books/{Id}/pricehistory");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UpdateBookPriceRequest request, CancellationToken ct)
  {
    await _bookService.UpdateBookPriceAsync(request.Id, request.Price, ct);

    var updatedBook = await _bookService.GetBookByIdAsync(request.Id, ct);

    if (updatedBook is null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await SendAsync(updatedBook, StatusCodes.Status200OK, ct);
  }
}
