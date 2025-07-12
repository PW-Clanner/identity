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
    public async Task<ActionResult<LoginUserCommandResponse>> Login([FromBody] LoginUserCommand command)
    {
        var result = await Mediatr.Send(command);

        hydraChallenge.Subject = result.Success ? result.UserId : "";

        return Ok(result);
    }
}