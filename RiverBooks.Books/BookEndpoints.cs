using Microsoft.AspNetCore.Builder;

namespace RiverBooks.Books;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("/books", (IBookService service) => service.ListBooks());
    }
}
