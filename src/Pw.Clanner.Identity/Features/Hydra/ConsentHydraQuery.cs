using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pw.Clanner.Identity.Features.Hydra;

public class ConsentHydraQuery : IRequest<bool>
{
    [FromQuery(Name = "consent_challenge")] 
    public string ConsentChallenge { get; set; }
}