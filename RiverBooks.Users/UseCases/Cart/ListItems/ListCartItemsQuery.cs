using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.UseCases.Cart.ListItems;

public record ListCartItemsQuery(string EmailAddress) : IRequest<Result<List<CartItemDto>>>;
