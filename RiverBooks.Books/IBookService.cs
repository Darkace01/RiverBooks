
namespace RiverBooks.Books;

internal interface IBookService
{
    IEnumerable<BookDto> ListBooks();
}