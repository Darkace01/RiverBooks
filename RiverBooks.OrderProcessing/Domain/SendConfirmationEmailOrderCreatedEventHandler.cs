using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Domain;

internal class SendConfirmationEmailOrderCreatedEventHandler(IMediator _mediator, ILogger<SendConfirmationEmailOrderCreatedEventHandler> _logger)
  : INotificationHandler<OrderCreatedEvent>
{
  public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
  {
    var userByIdQuery = new UserDetailsByIdQuery(notification.Order.UserId);

    var result = await _mediator.Send(userByIdQuery, cancellationToken);
    if (!result.IsSuccess)
    {
      _logger.LogInformation("Could not send email to user {UserId} for order {OrderId}.", notification.Order.UserId, notification.Order.Id);
      return;
    }

    string userEmail = result.Value.EmailAddress;

    var command = new SendEmailCommand()
    {
      To = userEmail,
      From = "noreply@test.com",
      Subject = "Your RiverBooks Purchase",
      Body = $"Thank you for your purchase of {notification.Order.OrderItems.Count} items from RiverBooks."
    };

    Guid emailId = await _mediator.Send(command, cancellationToken);

    _logger.LogInformation("Email sent to {To} with id {EmailId} for order {OrderId}.", userEmail, emailId, notification.Order.Id);
  }
}
