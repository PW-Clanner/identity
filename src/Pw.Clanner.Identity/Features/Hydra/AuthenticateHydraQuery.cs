using MediatR;

namespace Pw.Clanner.Identity.Features.Hydra;

public class AuthenticateHydraQuery : IRequest<AuthenticateHydraQueryResponse>;