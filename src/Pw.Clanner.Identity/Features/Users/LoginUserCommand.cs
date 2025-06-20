using MediatR;

namespace Pw.Clanner.Identity.Features.Users;

public record LoginUserCommand(string UserName, string Password, bool RememberMe)
    : IRequest<string>;