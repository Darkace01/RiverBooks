using Ardalis.Result;
using MongoDB.Driver;

namespace RiverBooks.EmailSending;

internal class MongoDbOutboxService(IMongoCollection<EmailOutboxEntity> _emailCollection) : IOutboxService
{
  public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
  {
    var filter = Builders<EmailOutboxEntity>.Filter.Eq(e => e.DateTimeUtcProcessed, null);
    var unsentEmailEntity = await _emailCollection.FindAsync(filter).Result.FirstOrDefaultAsync();

    if (unsentEmailEntity is null) return Result.NotFound();

    return unsentEmailEntity;
  }

  public async Task QueueEmailForSending(EmailOutboxEntity entity)
  {
    await _emailCollection.InsertOneAsync(entity);
  }
}
