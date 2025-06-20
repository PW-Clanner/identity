using Microsoft.AspNetCore.Http;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Infrastructure.Services;

public class CurrentHydraChallenge(IHttpContextAccessor httpContextAccessor) : ICurrentHydraChallenge
{
    public string LoginChallenge => httpContextAccessor.HttpContext?.Request?.Query["loginChallenge"];
    public string ConsentChallenge => httpContextAccessor.HttpContext?.Request?.Query["consentChallenge"];
}