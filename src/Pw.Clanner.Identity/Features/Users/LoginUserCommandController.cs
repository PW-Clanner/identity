using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pw.Clanner.Identity.Common;
using Pw.Clanner.Identity.Common.Interfaces;

namespace Pw.Clanner.Identity.Features.Users;

public class LoginUserCommandController(
    ICurrentHydraChallenge hydraChallenge,
    ILogger<LoginUserCommandController> logger) : ApiControllerBase
{
    [HttpPost("/api/users/login")]
    public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserCommand command)
    {
        var result = await Mediatr.Send(command);
        logger.LogInformation("LUCC hydra login_challenge = {@LoginChallenge}", hydraChallenge.LoginChallenge);

        return Ok(new LoginUserResponse(!string.IsNullOrWhiteSpace(result)));
    }
}