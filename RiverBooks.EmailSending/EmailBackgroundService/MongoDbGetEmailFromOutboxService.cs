using Ardalis.Result;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal class MongoDbGetEmailFromOutboxService(IMongoCollection<EmailOutboxEntity> _emailCollection) : IGetEmailsFromOutboxService
{
  public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
  {
    var filter = Builders<EmailOutboxEntity>.Filter.Eq(e => e.DateTimeUtcProcessed, null);
    var unsentEmailEntity = await _emailCollection.FindAsync(filter).Result.FirstOrDefaultAsync();

    if (unsentEmailEntity is null) return Result.NotFound();

    return unsentEmailEntity;
  }
}
