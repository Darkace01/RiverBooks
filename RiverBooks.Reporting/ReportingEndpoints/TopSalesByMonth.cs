using FastEndpoints;

namespace RiverBooks.Reporting.ReportingEndpoints;
internal record TopSalesByMonthRequest(int Year, int Month);
internal record TopSalesByMonthResponse(string Report);
internal class TopSalesByMonth :
  Endpoint<TopSalesByMonthRequest, TopSalesByMonthResponse>
{
  public override void Configure()
  {
    Get("/topsales");
    AllowAnonymous(); // TODOs: Lock down
  }

    public override async Task HandleAsync(
        TopSalesByMonthRequest request, CancellationToken ct)
    {
        var response = new TopSalesByMonthResponse("Hello, World!")
        {
            Report = "Hello, World!"
        };
        await SendAsync(response, cancellation: ct);
    }
}
