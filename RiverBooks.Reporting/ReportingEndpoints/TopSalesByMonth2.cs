using FastEndpoints;

namespace RiverBooks.Reporting;

internal class TopSalesByMonth2(ISalesReportService reportService) :
  Endpoint<TopSalesByMonthRequest, TopSalesByMonthResponse>
{
  public override void Configure()
  {
    Get("/topsales2");
    AllowAnonymous(); // TODOs: lock down
  }

  public override async Task HandleAsync(
  TopSalesByMonthRequest request,
  CancellationToken ct)
  {
    var report = await reportService.GetTopBooksByMonthReportAsync(
      request.Month, request.Year);
    var response = new TopSalesByMonthResponse { Report = report };
    await SendAsync(response, cancellation: ct);
  }
}
