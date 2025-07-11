using Microsoft.AspNetCore.Http;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Infrastructure.Services;

public class CurrentHydraChallenge(IHttpContextAccessor httpContextAccessor) : ICurrentHydraChallenge
{
    public string LoginChallenge => httpContextAccessor.HttpContext?.Request?.Query["login_challenge"] ??
                                    httpContextAccessor.HttpContext?.Items["login_challenge"] as string;
    public string ConsentChallenge => httpContextAccessor.HttpContext?.Request?.Query["consent_challenge"] ??
                                      httpContextAccessor.HttpContext?.Items["consent_challenge"] as string;
    
    public void StoreLoginChallenge()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
            return;

        httpContext.Items["login_challenge"] = LoginChallenge;
        httpContext.Items["consent_challenge"] = LoginChallenge;
    }
}