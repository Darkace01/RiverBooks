using Ardalis.GuardClauses;

namespace RiverBooks.Users;

public class CartItem
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid BookId { get; set; }
  public string Description { get; private set; } = string.Empty;
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }

  public CartItem()
  {
    // EF
  }

  public CartItem(Guid bookId, string description, int quantity, decimal unitPrice)
  {
    BookId = Guard.Against.Default(bookId);
    Description = Guard.Against.NullOrEmpty(description);
    Quantity = Guard.Against.Negative(quantity);
    UnitPrice = Guard.Against.Negative(unitPrice);
  }
  public void UpdateQuantity(int quantity)
  {
    Quantity = Guard.Against.Negative(quantity);
  }

  public void UpdateDescription(string description)
  {
    Description = Guard.Against.NullOrEmpty(description);
  }

  public void UpdateUnitPrice(decimal unitPrice)
  {
    UnitPrice = Guard.Against.Negative(unitPrice);
  }
}
