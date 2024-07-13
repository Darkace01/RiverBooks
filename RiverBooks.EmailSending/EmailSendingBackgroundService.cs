using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RiverBooks.EmailSending;

internal class EmailSendingBackgroundService(ILogger<EmailSendingBackgroundService> logger, ISendEmailsFromOutboxService sendEmailsFromOutboxService) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    int delayMilliseconds = 10_000; // 10 seconds

    logger.LogInformation("{ServiceName} is starting.", nameof(EmailSendingBackgroundService));

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await sendEmailsFromOutboxService.CheckForAndSendEmails();
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error processing outbox: {Message}", ex.Message);
      }
      finally
      {
        await Task.Delay(delayMilliseconds, stoppingToken);
      }
      logger.LogInformation("{ServiceName} is stopping.", nameof(EmailSendingBackgroundService));
    }
  }
}
