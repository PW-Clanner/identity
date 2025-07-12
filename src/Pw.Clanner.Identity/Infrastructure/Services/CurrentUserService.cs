using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Infrastructure.Services;

public class CurrentHydraChallenge(IHttpContextAccessor httpContextAccessor, ILogger<CurrentHydraChallenge> logger)
    : ICurrentHydraChallenge
{
    public string LoginChallenge
    {
        get => httpContextAccessor.HttpContext?.Session.Keys.Contains("login_challenge") == true
            ? httpContextAccessor.HttpContext?.Session.GetString("login_challenge")
            : null;
        set => StoreLoginChallenge(value);
    }

    public string Subject
    {
        get => httpContextAccessor.HttpContext?.Session.Keys.Contains("subject") == true
            ? httpContextAccessor.HttpContext?.Session.GetString("subject")
            : null;
        set => StoreSubject(value);
    }

    public string ConsentChallenge
    {
        get => httpContextAccessor.HttpContext?.Session.Keys.Contains("consent_challenge") == true
            ? httpContextAccessor.HttpContext?.Session.GetString("consent_challenge")
            : null;
        set => StoreConsentChallenge(value);
    }

    private void StoreLoginChallenge(string loginChallenge)
    {
        logger.LogInformation("session store LoginChallenge {LoginChallenge}", loginChallenge);
        httpContextAccessor.HttpContext?.Session.SetString("login_challenge", loginChallenge);
    }

    private void StoreSubject(string subject)
    {
        logger.LogInformation("session store Subject {Subject}", subject);
        httpContextAccessor.HttpContext?.Session.SetString("subject", subject);
    }

    private void StoreConsentChallenge(string consentChallenge)
    {
        logger.LogInformation("session store LoginChallenge {ConsentChallenge}", consentChallenge);
        httpContextAccessor.HttpContext?.Session.SetString("consent_challenge", consentChallenge);
    }
}