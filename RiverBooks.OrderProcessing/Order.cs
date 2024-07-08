﻿namespace RiverBooks.OrderProcessing;

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

  internal static class Factory
  {
    public static Order Create(Guid userId, Address shippingAddress, Address billingAddress, IEnumerable<OrderItem> orderItems)
    {
      var order = new Order
      {
        UserId = userId,
        ShippingAddress = shippingAddress,
        BillingAddress = billingAddress
      };
      foreach (var item in orderItems)
      {
        order.AddOrderItem(item);
      }
      return order;
    }
  }
}
