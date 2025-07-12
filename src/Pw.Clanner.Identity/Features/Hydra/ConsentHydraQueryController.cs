using Microsoft.AspNetCore.Mvc;
using Ory.Hydra.Client.Api;
using Ory.Hydra.Client.Model;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Hydra;

public class ConsentHydraQueryController(ICurrentHydraChallenge hydraChallenge, IOAuth2ApiAsync oauth2Api)
    : ApiControllerBase
{
    [HttpGet("/hydra/consent")]
    public async Task<ActionResult> ConsentHydra(ConsentHydraQuery query, CancellationToken cancellationToken)
    {
        hydraChallenge.ConsentChallenge = query.ConsentChallenge;
        var consentRequest =
            await oauth2Api.GetOAuth2ConsentRequestAsync(query.ConsentChallenge, cancellationToken: cancellationToken);

        var acceptResponse = await oauth2Api.AcceptOAuth2ConsentRequestAsync(query.ConsentChallenge,
            new HydraAcceptOAuth2ConsentRequest(grantScope: consentRequest.RequestedScope,
                grantAccessTokenAudience: consentRequest.RequestedAccessTokenAudience),
            cancellationToken: cancellationToken);

        return Redirect(acceptResponse.RedirectTo);
    }
}