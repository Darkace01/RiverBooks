using Ardalis.Result;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace RiverBooks.EmailSending.EmailBackgroundService;
internal interface IGetEmailsFromOutboxService
{
  Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity();
}

internal class DefaultSendEmailsFromOutboxService(IGetEmailsFromOutboxService outboxService,
                                                  ISendEmail emailSender,
                                                  IMongoCollection<EmailOutboxEntity> emailCollection,
                                                  ILogger<DefaultSendEmailsFromOutboxService> logger) : ISendEmailsFromOutboxService
{
  public async Task CheckForAndSendEmails()
  {
    try
    {


      var result = await outboxService.GetUnprocessedEmailEntity();

      if (result.Status == ResultStatus.NotFound) return;

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
