using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Hydra;

public class LoginHydraQueryController(ICurrentHydraChallenge hydraChallenge, ILogger<LoginHydraQueryController> logger)
    : ApiControllerBase
{
    [HttpGet("/hydra/login")]
    public async Task<ActionResult> LoginHydra(LoginHydraQuery command)
    {
        var result = await Mediatr.Send(command);

        if (result)
            return Redirect("https://auth.clanner.pw");

        return BadRequest("???");
    }
}