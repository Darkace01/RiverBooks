using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;

internal class AddressCacheUpdatingNewUserAddressHandler(IOrderAddressCache _addressCache,
                                                         ILogger<AddressCacheUpdatingNewUserAddressHandler> _logger) 
                                                        : INotificationHandler<NewUserAddressIntegrationEvent>
{
  public async Task Handle(NewUserAddressIntegrationEvent notification, CancellationToken cancellationToken)
  {
    var orderAddress = new OrderAddress(notification.Details.AddressId, new Address(notification.Details.Street1,
                                                                                    notification.Details.Street2,
                                                                                    notification.Details.City,
                                                                                    notification.Details.State,
                                                                                    notification.Details.PostalCode,
                                                                                    notification.Details.Country));

    await _addressCache.StoreAsync(orderAddress);

    _logger.LogInformation("Cached updated with new address {Address}", orderAddress);
  }
}
