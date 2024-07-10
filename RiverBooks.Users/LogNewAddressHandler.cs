using MediatR;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Users;
internal class LogNewAddressHandler(ILogger<LogNewAddressHandler> _logger) : INotificationHandler<AddressAddedEvent>
{
  public Task Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("[DE Handler] Address added for user {UserId} : {Address}",
                           notification.NewAddress.UserId,
                           notification.NewAddress.StreetAddress);

    return Task.CompletedTask;
  }
}
