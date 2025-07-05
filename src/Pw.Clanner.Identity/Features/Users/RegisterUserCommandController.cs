using Microsoft.AspNetCore.Mvc;
using Pw.Clanner.Identity.Common;

namespace Pw.Clanner.Identity.Features.Users;

public class RegisterUserCommandController : ApiControllerBase
{
    [HttpPost("/api/users/register")]
    public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserCommand command)
    {
        var result = await Mediatr.Send(command);

        return Ok(new RegisterUserResponse(!string.IsNullOrWhiteSpace(result)));
    }
}