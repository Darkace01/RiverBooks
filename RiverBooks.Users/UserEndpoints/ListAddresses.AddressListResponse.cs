﻿using RiverBooks.Users.CartEndpoints;

namespace RiverBooks.Users.UserEndpoints;

public class AddressListResponse
{
  public List<UserAddressDto> Addresses { get; set; } = [];
}
