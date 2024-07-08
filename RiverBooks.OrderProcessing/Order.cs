namespace RiverBooks.OrderProcessing;

internal class Order
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid UserId { get; set; }
  public Address ShippingAddress { get; private set; } = default!;
  public Address BillingAddress { get; private set; } = default!;
  private readonly List<OrderItem> _orderItems = [];
  public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
  public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;

  internal void AddOrderItem(OrderItem item) => _orderItems.Add(item);
}
