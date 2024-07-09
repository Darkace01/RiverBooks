using System.Net.Sockets;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser
{
  public string FullName { get; set; } = string.Empty;
  private readonly List<CartItem> _cartItems = [];
  public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

  private readonly List<UserStreetAddress> _addresses = [];
  public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

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

  internal UserStreetAddress AddAdress(Address address)
  {
    Guard.Against.Null(address);

    // find existing address and just return it
    var existingAddress = _addresses.SingleOrDefault(a => a.StreetAddress == address);
    if(existingAddress is not null)
    {
      return existingAddress;
    }

    var newAddress = new UserStreetAddress(Id, address);
    _addresses.Add(newAddress);

    return newAddress;
  }

  public void ClearCart()
  {
    _cartItems.Clear();
  }
}
