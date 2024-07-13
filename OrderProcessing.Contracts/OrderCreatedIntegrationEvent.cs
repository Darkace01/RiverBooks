using MediatR;

namespace OrderProcessing.Contracts;

public class OrderCreatedIntegrationEvent : INotification
{
  public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
  public OrderDetailsDto OrderDetails { get; private set; }

  public OrderCreatedIntegrationEvent(OrderDetailsDto orderDetails)
  {
    OrderDetails = orderDetails;
  }
}
