using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pw.Clanner.Identity.Features.Hydra;

public class LoginHydraQuery : IRequest<bool>
{
    [FromQuery(Name = "login_challenge")] 
    public string LoginChallenge { get; set; }
}