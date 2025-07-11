using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Hydra;

public class LoginHydraQueryController(ICurrentHydraChallenge hydraChallenge, ILogger<LoginHydraQueryController> logger)
    : ApiControllerBase
{
    [HttpGet("/hydra/login")]
    public async Task<ActionResult> GetUserAudit()
    {
        logger.LogInformation("HYDRA REQUEST {LoginChallenge}", hydraChallenge.LoginChallenge);
        return Redirect("https://auth.clanner.pw");
    }
}