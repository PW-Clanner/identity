using Microsoft.AspNetCore.Mvc;
using Ory.Hydra.Client.Api;
using Ory.Hydra.Client.Model;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Hydra;

public class AuthenticateHydraQueryController(ICurrentHydraChallenge hydraChallenge, IOAuth2ApiAsync oauth2Api)
    : ApiControllerBase
{
    [HttpGet("/hydra/authenticate")]
    public async Task<ActionResult> AuthenticateHydra()
    {
        var response = string.IsNullOrEmpty(hydraChallenge.Subject)
            ? await oauth2Api.RejectOAuth2LoginRequestAsync(hydraChallenge.LoginChallenge)
            : await oauth2Api.AcceptOAuth2LoginRequestAsync(hydraChallenge.LoginChallenge,
                new HydraAcceptOAuth2LoginRequest(subject: hydraChallenge.Subject));

        return Redirect(response.RedirectTo);
    }
}