namespace Pw.Clanner.Identity.Features.Users;

public record LoginUserCommandResponse(bool Success, string RedirectTo);