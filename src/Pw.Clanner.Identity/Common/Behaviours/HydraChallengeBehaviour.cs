using System.Reflection;
using MediatR;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Common.Security;

namespace Pw.Clanner.Identity.Common.Behaviours;

public class HydraChallengeBehaviour<TRequest, TResponse>(ICurrentHydraChallenge currentHydraChallenge)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var hydraChallengeAttributes = request
            .GetType()
            .GetCustomAttributes<HydraChallengeAttribute>()
            .ToList();

        if (hydraChallengeAttributes.Count == 0)
            return next(cancellationToken);

        var loginChallenge = currentHydraChallenge.LoginChallenge ?? string.Empty;
        var consentChallenge = currentHydraChallenge.ConsentChallenge ?? string.Empty;

        var hasLoginChallenge = !string.IsNullOrWhiteSpace(loginChallenge);
        var hasConsentChallenge = !string.IsNullOrWhiteSpace(consentChallenge);

        foreach (var hydraChallengeAttribute in hydraChallengeAttributes)
        {
            if (hydraChallengeAttribute.NeedLoginChallenge && !hasLoginChallenge)
                throw new UnauthorizedAccessException("LoginChallenge не задан");

            if (hydraChallengeAttribute.NeedConsentChallenge && !hasConsentChallenge)
                throw new UnauthorizedAccessException("LoginChallenge не задан");
        }

        return next(cancellationToken);
    }
}