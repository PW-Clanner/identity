using Microsoft.AspNetCore.Mvc;
using Ory.Hydra.Client.Api;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Hydra;

public class ConsentHydraQueryController(ICurrentHydraChallenge hydraChallenge, IOAuth2ApiAsync oauth2Api)
    : ApiControllerBase
{
    [HttpGet("/hydra/consent")]
    public async Task<ActionResult> ConsentHydra(ConsentHydraQuery query)
    {
        hydraChallenge.ConsentChallenge = query.ConsentChallenge;

        var acceptResponse = await oauth2Api.AcceptOAuth2ConsentRequestAsync(hydraChallenge.ConsentChallenge);

        return Redirect(acceptResponse.RedirectTo);
    }
}