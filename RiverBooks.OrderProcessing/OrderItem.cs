using Ardalis.GuardClauses;

namespace RiverBooks.OrderProcessing;

internal class OrderItem
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid BookId { get; set; }
  public string Description { get; private set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }


  public OrderItem(Guid bookId, int quantity, decimal unitPrice, string description)
  {
    BookId = Guard.Against.Default(bookId);
    Quantity = Guard.Against.NegativeOrZero(quantity);
    UnitPrice = Guard.Against.NegativeOrZero(unitPrice);
    Description = Guard.Against.NullOrEmpty(description);
  }
}
