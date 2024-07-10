using FastEndpoints;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing.Endpoints;
/// <summary>
/// Used for testing only
/// </summary>
/// <param name="_db"></param>
/// <param name="_logger"></param>
internal class FlushCache: EndpointWithoutRequest
{
  private readonly ILogger<FlushCache> _logger;
  private readonly IDatabase _db;

  public FlushCache( ILogger<FlushCache> logger)
  {
    var redis = ConnectionMultiplexer.Connect("localhost");
    _db = redis.GetDatabase();
    _logger = logger;
  }

  public override void Configure()
  {
    Post("/flushcache");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await _db.ExecuteAsync("FLUSHDB");
    _logger.LogWarning("FLUSHED CACHE FOR {Db}", "REDIS");
  }
}
