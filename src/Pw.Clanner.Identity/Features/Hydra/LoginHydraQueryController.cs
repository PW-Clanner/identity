using Microsoft.AspNetCore.Mvc;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;
using Pw.Clanner.Identity.Common.Security;

namespace Pw.Clanner.Identity.Features.Hydra;

[HydraChallenge(false, true)]
public class LoginHydraQueryController(ICurrentHydraChallenge hydraChallenge) : ApiControllerBase
{
    [HttpGet("/hydra/login")]
    public async Task<ActionResult> GetUserAudit()
    {
        hydraChallenge.StoreLoginChallenge();
        return Redirect("https://auth.clanner.pw");
    }
}