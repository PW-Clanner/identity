using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Common.Behaviours;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, ICurrentHydraChallenge currentHydraChallenge)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger = logger;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var loginChallenge = currentHydraChallenge.LoginChallenge ?? string.Empty;
        var consentChallenge = currentHydraChallenge.ConsentChallenge ?? string.Empty;

        return Task.Run(
            () => _logger.LogInformation(
                "Запрос: {Name} {@LoginChallenge} {@ConsentChallenge}, {@Request}",
                requestName,
                loginChallenge,
                consentChallenge,
                request),
            cancellationToken);
    }
}