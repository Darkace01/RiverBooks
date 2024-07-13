using MediatR;
using Microsoft.Extensions.Logging;
using OrderProcessing.Contracts;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Reporting.Integrations;
internal class NewOrderCreatedIngestionHandler(ILogger<NewOrderCreatedIngestionHandler> logger,
  OrderIngestionService orderIngestionService,
  IMediator mediator) :
  INotificationHandler<OrderCreatedIntegrationEvent>
{
  public async Task Handle(OrderCreatedIntegrationEvent notification,
    CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling order created event to populate reporting database...");

    var orderItems = notification.OrderDetails.OrderItems;
    int year = notification.OrderDetails.DateCreated.Year;
    int month = notification.OrderDetails.DateCreated.Month;

    foreach (var item in orderItems)
    {
      // look up book details to get author and title
      // TODOs: Implement Materialized View or other cache
      var bookDetailsQuery = new BookDetailsQuery(item.BookId);
      var result = await mediator.Send(bookDetailsQuery, cancellationToken);

      if (!result.IsSuccess)
      {
        logger.LogWarning("Issue loading book details for {Id}", item.BookId);
        continue;
      }

      string author = result.Value.Author;
      string title = result.Value.Title;

      var sale = new BookSale
      {
        Author = author,
        BookId = item.BookId,
        Month = month,
        Title = title,
        Year = year,
        TotalSales = item.Quantity * item.UnitPrice,
        UnitsSold = item.Quantity
      };

      await orderIngestionService.AddOrUpdateMonthlyBookSalesAsync(sale);
    }
  }
}
