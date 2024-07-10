
using System.Text.Json;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing;

internal interface IOrderAddressCache
{
  Task<Result<OrderAddress>> GetByIdAsync(Guid id);
  Task<Result> StoreAsync(OrderAddress orderAddress);
}

internal class RedisOrderAddressCache :
  IOrderAddressCache
{
  private readonly ILogger<RedisOrderAddressCache> _logger;
  private readonly IDatabase _db;

  public RedisOrderAddressCache(ILogger<RedisOrderAddressCache> logger)
  {
    _logger = logger;
    var redis = ConnectionMultiplexer.Connect("localhost");
    _db = redis.GetDatabase();
  }


  public async Task<Result<OrderAddress>> GetByIdAsync(Guid id)
  {
    string? fetchedJson = await _db.StringGetAsync(id.ToString());

    if (fetchedJson is null)
    {
      _logger.LogWarning("Address {Id} not found in {Db}", id, "REDIS");
      return Result.NotFound();
    }
    var address = JsonSerializer.Deserialize<OrderAddress>(fetchedJson);

    return Result.Success(address!);
  }

  public async Task<Result> StoreAsync(OrderAddress orderAddress)
  {
    var key = orderAddress.Id.ToString();
    var addressJson = JsonSerializer.Serialize(orderAddress);

    await _db.StringSetAsync(key, addressJson);
    _logger.LogInformation("Address {Id} stored in {Db}", orderAddress.Id, "REDIS");

    return Result.Success();
  }
}
