using FastEndpoints;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.ListEmailsEndpoint;
public class ListEmailsResponse
{
  public int Count { get; set; }
  public List<EmailOutboxEntity> Emails { get; set; } = [];
}

internal class ListEmails(IMongoCollection<EmailOutboxEntity> emailCollection) : EndpointWithoutRequest<ListEmailsResponse>
{
  public override void Configure()
  {
    Get("/emails");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var filter = Builders<EmailOutboxEntity>.Filter.Empty;
    var emailEntities = await emailCollection.FindAsync(filter, cancellationToken: ct).Result
                                             .ToListAsync(ct);

    var response = new ListEmailsResponse
    {
      Count = emailEntities.Count,
      Emails = emailEntities
    };

    Response = response;
  }
}
