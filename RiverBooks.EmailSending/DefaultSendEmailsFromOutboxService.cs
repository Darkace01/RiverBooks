using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace RiverBooks.EmailSending;

internal class DefaultSendEmailsFromOutboxService(IOutboxService outboxService,
                                                  ISendEmail emailSender,
                                                  IMongoCollection<EmailOutboxEntity> emailCollection,
                                                  ILogger<DefaultSendEmailsFromOutboxService> logger) : ISendEmailsFromOutboxService
{
  public async Task CheckForAndSendEmails()
  {
    try
    {


      var result = await outboxService.GetUnprocessedEmailEntity();

      if (result.Status == Ardalis.Result.ResultStatus.NotFound) return;

      var emailEntity = result.Value;

      await emailSender.SendEmailAsync(emailEntity.To, emailEntity.From, emailEntity.Subject, emailEntity.Body);

      var updateFilter = Builders<EmailOutboxEntity>.Filter.Eq(e => e.Id, emailEntity.Id);
      var update = Builders<EmailOutboxEntity>.Update.Set(e => e.DateTimeUtcProcessed, DateTime.UtcNow);
      var updatedResult = await emailCollection.UpdateOneAsync(updateFilter, update);
      logger.LogInformation("Processed {Result} email records", updatedResult.ModifiedCount);
    }
    finally
    {
      logger.LogInformation("Sleeping...");
    }
  }
}
