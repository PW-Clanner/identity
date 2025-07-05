using Microsoft.AspNetCore.Mvc;
using Pw.Clanner.Identity.Common;

namespace Pw.Clanner.Identity.Features.Users;

public class LoginUserCommandController : ApiControllerBase
{
    [HttpPost("/api/users/login")]
    public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserCommand command)
    {
        var result = await Mediatr.Send(command);

        return Ok(new LoginUserResponse(!string.IsNullOrWhiteSpace(result)));
    }
}