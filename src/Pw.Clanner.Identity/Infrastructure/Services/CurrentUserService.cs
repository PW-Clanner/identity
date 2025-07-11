using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Infrastructure.Services;

public class CurrentHydraChallenge(IHttpContextAccessor httpContextAccessor, ILogger<CurrentHydraChallenge> logger)
    : ICurrentHydraChallenge
{
    public string LoginChallenge
    {
        get
        {
            var query = httpContextAccessor.HttpContext?.Request?.Query["login_challenge"];
            if (query.HasValue)
            {
                StoreLoginChallenge(query);
                return query;
            }

            if (httpContextAccessor.HttpContext?.Session.Keys.Contains("login_challenge") == true)
            {
                return httpContextAccessor.HttpContext?.Session.GetString("login_challenge");
            }

            return null;
        }
    }

    public string ConsentChallenge
    {
        get
        {
            var query = httpContextAccessor.HttpContext?.Request?.Query["consent_challenge"];
            if (query.HasValue)
            {
                StoreConsentChallenge(query);
                return query;
            }

            if (httpContextAccessor.HttpContext?.Session.Keys.Contains("consent_challenge") == true)
            {
                return httpContextAccessor.HttpContext?.Session.GetString("consent_challenge");
            }

            return null;
        }
    }

    private void StoreLoginChallenge(string loginChallenge)
    {
        logger.LogInformation("session store LoginChallenge {LoginChallenge}", loginChallenge);
        httpContextAccessor.HttpContext?.Session.SetString("login_challenge", loginChallenge);
    }

    private void StoreConsentChallenge(string consentChallenge)
    {
        logger.LogInformation("session store LoginChallenge {ConsentChallenge}", consentChallenge);
        httpContextAccessor.HttpContext?.Session.SetString("consent_challenge", consentChallenge);
    }
}