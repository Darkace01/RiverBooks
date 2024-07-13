using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace RiverBooks.EmailSending;
public class MimeKitEmailSender(ILogger<MimeKitEmailSender> logger) : ISendEmail
{
  public async Task SendEmailAsync(string to, string from, string subject, string body)
  {
    logger.LogInformation("Attempting to send email to {To} from {From} with subject {Subject}", to, from, subject);

    using (var client = new SmtpClient())
    {  // use localhost and a test server

      await client.ConnectAsync(Constants.EMAIL_SERVER, 25, false); //TODOs: fetch from config
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(from, from));
      message.To.Add(new MailboxAddress(to, to));
      message.Subject = subject;
      message.Body = new TextPart("plain")
      {
        Text = body
      };

      await client.SendAsync(message);
      logger.LogInformation("Email sent to {To} from {From} with subject {Subject}", to, from, subject);

#pragma warning disable S6966 // Awaitable method should be used
      client.Disconnect(true);
#pragma warning restore S6966 // Awaitable method should be used

      logger.LogInformation("Sent to {To}", to);
    }
  }
}
