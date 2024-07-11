using System.Diagnostics;
using System.Reflection;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RiverBooks.SharedKernel;

public class LogginBehavior<TRequest, TResponse>(ILogger<LogginBehavior<TRequest, TResponse>> logger) :
  IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public async Task<TResponse> Handle(TRequest request,
                                      RequestHandlerDelegate<TResponse> next,
                                      CancellationToken cancellationToken)
  {
    Guard.Against.Null(request);
    if (logger.IsEnabled(LogLevel.Information))
    {
      logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

      // Reflection! Could be a performance concern
      var myType = request.GetType();
      IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
      foreach (var prop in props)
      {
        var propValue = prop?.GetValue(request, null);
        logger.LogInformation("{Property} : {@Value}", prop?.Name, propValue);
      }
    }
    var sw = Stopwatch.StartNew();

    var response = await next();

    logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms",
                           typeof(TRequest).Name, sw.ElapsedMilliseconds);

    sw.Stop();
    return response;

  }
}
