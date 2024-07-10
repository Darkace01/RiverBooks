using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser, IHaveDomainEvents
{
  public string FullName { get; set; } = string.Empty;
  private readonly List<CartItem> _cartItems = [];
  public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

  private readonly List<UserStreetAddress> _addresses = [];
  public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

  private readonly List<DomainEventBase> _domainEvents = [];
  [NotMapped]
  public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  void IHaveDomainEvents.CLearDomainEvents() => _domainEvents.Clear();
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
    if (existingAddress is not null)
    {
      return existingAddress;
    }

    var newAddress = new UserStreetAddress(Id, address);
    _addresses.Add(newAddress);

    var domainEvent = new AddressAddedEvent(newAddress);
    RegisterDomainEvent(domainEvent);

    return newAddress;
  }

  public void ClearCart()
  {
    _cartItems.Clear();
  }
}
