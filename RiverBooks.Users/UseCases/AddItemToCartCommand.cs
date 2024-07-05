using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Users.UseCases;

public record AddItemToCartCommand(Guid BookId, int Quantity, string EmailAddress)
  : IRequest<Result>;

public class AddItemToCartCommandHandler(IApplicationUserRepository _userRepository, IMediator _mediator) : IRequestHandler<AddItemToCartCommand, Result>
{
  public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddress, cancellationToken);

    if (user is null)
    {
      return Result.Unauthorized();
    }

    var bookQuery = new BookDetailsQuery(request.BookId);
    var bookDetailsRequest = await _mediator.Send(bookQuery, cancellationToken);

    if (bookDetailsRequest.Status == ResultStatus.NotFound) return Result.NotFound();
    var bookDetails = bookDetailsRequest.Value;

    string decription = $"{bookDetails.Title} by {bookDetails.Author}";

    var newCartItem = new CartItem(request.BookId,
                                   decription,
                                   request.Quantity,
                                   bookDetails.Price);
    user.AddItemToCart(newCartItem);
    await _userRepository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}
