using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases;

public record AddItemToCartCommand(Guid BookId, int Quantity, string EmailAddress)
  : IRequest<Result>;

public class AddItemToCartCommandHandler(IApplicationUserRepository _userRepository) : IRequestHandler<AddItemToCartCommand, Result>
{
  public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddress, cancellationToken);

    if (user is null)
    {
      return Result.Unauthorized();
    }

    //TODOs: Get description and price from the book module
    var newCartItem = new CartItem(request.BookId,
                                   "description",
                                   request.Quantity,
                                   1.00m);
    user.AddItemToCart(newCartItem);
    await _userRepository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}
