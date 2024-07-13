using MongoDB.Driver;
using RiverBooks.EmailSending.Integrations;

namespace RiverBooks.EmailSending.EmailBackgroundService;

internal class MongoDbQueueEmailOutboxService(IMongoCollection<EmailOutboxEntity> _emailCollection) : IQueueEmailsInOutboxService
{
  public async Task QueueEmailForSending(EmailOutboxEntity entity)
  {
    await _emailCollection.InsertOneAsync(entity);
  }
}
