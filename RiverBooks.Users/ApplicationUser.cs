using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser
{
  public string FullName { get; set; } = string.Empty;
  private readonly List<CartItem> _cartItems = [];
  public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

  public void AddItemToCart(CartItem item)
  {
    Guard.Against.Null(item);

    var existingItem = _cartItems.SingleOrDefault(i => i.BookId == item.BookId);
    if (existingItem is not null)
    {
      existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
      //TODOs: What to do if other details of the item have been updated?
      existingItem.UpdateDescription(item.Description);
      existingItem.UpdateUnitPrice(item.UnitPrice);
      return;
    }
    _cartItems.Add(item);
  }
}
