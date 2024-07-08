namespace RiverBooks.OrderProcessing.Endpoints;

public class ListOrderForUserResponse
{
  public List<OrderSummary> Orders { get; set; } = [];
}
