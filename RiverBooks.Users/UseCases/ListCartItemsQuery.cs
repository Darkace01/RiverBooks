using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases;

public record ListCartItemsQuery(string EmailAddress) : IRequest<Result<List<CartItemDto>>>;

internal class ListCartItemsQueryHandler(IApplicationUserRepository userRepository) : IRequestHandler<ListCartItemsQuery, Result<List<CartItemDto>>>
{
  public async Task<Result<List<CartItemDto>>> Handle(ListCartItemsQuery request, CancellationToken cancellationToken)
  {
    var user = await userRepository.GetUserWithCartByEmailAsync(request.EmailAddress, cancellationToken);

    if (user is null)
    {
      return Result.Unauthorized();
    }

    return user.CartItems.Select(item => new CartItemDto(item.Id,
                                                         item.BookId,
                                                         item.Description,
                                                         item.Quantity,
                                                         item.UnitPrice))
                         .ToList();
  }
}
