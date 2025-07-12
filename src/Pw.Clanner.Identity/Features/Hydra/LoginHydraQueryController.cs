using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ory.Hydra.Client.Api;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Hydra;

public class LoginHydraQueryController(ICurrentHydraChallenge hydraChallenge, IOAuth2ApiAsync oauth2Api)
    : ApiControllerBase
{
    [HttpGet("/hydra/login")]
    public async Task<ActionResult> LoginHydra(LoginHydraQuery query)
    {
        var result = await Mediatr.Send(query);

        if (result)
            return Redirect("https://auth.clanner.pw");

        return BadRequest("???");
    }
}