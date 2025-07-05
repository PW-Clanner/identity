using Microsoft.AspNetCore.Mvc;
using Pw.Clanner.Identity.Common;

namespace Pw.Clanner.Identity.Features.Users;

public class LoginUserCommandController : ApiControllerBase
{
    [HttpPost("/api/users/login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command)
    {
        return await Mediatr.Send(command);
    }
}