using Microsoft.AspNetCore.Identity;
using Pw.Clanner.Identity.Domain.Entities.Users;

namespace Pw.Clanner.Identity.Common;

public static class UserExtensions
{
    private static readonly PasswordHasher<User> Hasher = new();

    public static void GeneratePasswordHash(this User user, string password)
    {
        var hash = Hasher.HashPassword(user, password);
        user.PasswordHash = hash;
    }

    public static bool VerifyPasswordHash(this User user, string password)
    {
        var result = Hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}