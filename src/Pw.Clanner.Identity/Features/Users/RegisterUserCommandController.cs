using Microsoft.AspNetCore.Mvc;
using Pw.Clanner.Identity.Common;

namespace Pw.Clanner.Identity.Features.Users;

public class RegisterUserCommandController : ApiControllerBase
{
    [HttpPost("/api/users/register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterUserCommand command)
    {
        return await Mediatr.Send(command);
    }
}