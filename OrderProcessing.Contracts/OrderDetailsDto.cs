namespace OrderProcessing.Contracts;

/// <summary>
/// Basic Details of the order
/// TODOs: Include address info for geographic specific reports to use
/// </summary>
public class OrderDetailsDto
{
  public Guid OrderId { get; set; }
  public Guid UserId { get; set; }
  public DateTimeOffset DateCreated { get; set; }
  public List<OrderItemDetails> OrderItems { get; set; } = [];
}
