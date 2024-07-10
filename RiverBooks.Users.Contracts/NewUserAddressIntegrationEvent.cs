namespace RiverBooks.Users.Contracts;

public record NewUserAddressIntegrationEvent(UserAddressDetails Details): IntegrationEventBase;
