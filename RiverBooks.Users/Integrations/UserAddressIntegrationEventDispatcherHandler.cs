using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Contracts;

namespace RiverBooks.Users.Integrations;
internal class UserAddressIntegrationEventDispatcherHandler(IMediator _mediator, ILogger<UserAddressIntegrationEventDispatcherHandler> _logger) :
  INotificationHandler<AddressAddedEvent>
{
  public async Task Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
  {
    Guid userId = Guid.Parse(notification.NewAddress.UserId);

    var addressDetails = new UserAddressDetails(
      userId,
      notification.NewAddress.Id,
      notification.NewAddress.StreetAddress.Street1,
      notification.NewAddress.StreetAddress.Street2,
      notification.NewAddress.StreetAddress.City,
      notification.NewAddress.StreetAddress.State,
      notification.NewAddress.StreetAddress.PostalCode,
      notification.NewAddress.StreetAddress.Country);

    await _mediator!.Publish(new NewUserAddressIntegrationEvent(addressDetails), cancellationToken);

    _logger.LogInformation("[DE Handler] Address added for user {UserId} : {Address}",
                           notification.NewAddress.UserId,
                           notification.NewAddress.StreetAddress);
  }
}
