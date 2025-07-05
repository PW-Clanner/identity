using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger,
    ICurrentHydraChallenge currentHydraChallenge)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next(cancellationToken);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500)
            return response;

        var requestName = typeof(TRequest).Name;
        var loginChallenge = currentHydraChallenge.LoginChallenge ?? string.Empty;
        var consentChallenge = currentHydraChallenge.ConsentChallenge ?? string.Empty;

        logger.LogWarning(
            "Запрос слишком долго выполнялся: {Name} ({ElapsedMilliseconds} мс) {@LoginChallenge} {@ConsentChallenge} {@Request}",
            requestName,
            elapsedMilliseconds,
            loginChallenge,
            consentChallenge,
            request);

        return response;
    }
}