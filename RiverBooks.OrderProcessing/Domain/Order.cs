using RiverBooks.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiverBooks.OrderProcessing.Domain;

internal class Order:IHaveDomainEvents
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid UserId { get; set; }
  public Address ShippingAddress { get; private set; } = default!;
  public Address BillingAddress { get; private set; } = default!;
  private readonly List<OrderItem> _orderItems = [];
  public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
  private readonly List<DomainEventBase> _domainEvents = [];
  [NotMapped]
  public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  void IHaveDomainEvents.CLearDomainEvents() => _domainEvents.Clear();
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
      var createdEvent = new OrderCreatedEvent(order);
      order.RegisterDomainEvent(createdEvent);

      return order;
    }
  }
}
