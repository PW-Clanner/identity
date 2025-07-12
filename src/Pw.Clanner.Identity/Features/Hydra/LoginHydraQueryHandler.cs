using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Hydra;

internal sealed class LoginHydraQueryHandler(
    ICurrentHydraChallenge hydraChallenge,
    IHttpContextAccessor httpContextAccessor,
    ILogger<LoginHydraQueryHandler> logger)
    : IRequestHandler<LoginHydraQuery, bool>
{
    public async Task<bool> Handle(LoginHydraQuery request, CancellationToken cancellationToken)
    {
        var query = httpContextAccessor.HttpContext?.Request.Query["login_challenge"];
        hydraChallenge.LoginChallenge = query;

        return true;
    }
}