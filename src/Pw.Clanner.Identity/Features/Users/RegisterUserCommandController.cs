using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Pw.Clanner.Identity.Common;

namespace Pw.Clanner.Identity.Features.Users;

public class RegisterUserCommandController : ApiControllerBase
{
    [HttpPost("/api/users/register")]
    [EnableRateLimiting("fixed")]
    public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserCommand command)
    {
        var result = await Mediatr.Send(command);

        return Ok(new RegisterUserResponse(!string.IsNullOrWhiteSpace(result)));
    }
}