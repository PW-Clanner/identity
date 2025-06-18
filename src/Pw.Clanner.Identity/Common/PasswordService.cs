using Microsoft.AspNetCore.Identity;
using Pw.Clanner.Identity.Domain.Entities;

namespace Pw.Clanner.Identity.Common;

public static class UserExtensions
{
    private static PasswordHasher<UserEntity> _hasher = new();

    public static void GeneratePasswordHash(this UserEntity user, string password)
    {
        var hash = _hasher.HashPassword(user, password);
        user.PasswordHash = hash;
    }

    public static bool VerifyPasswordHash(this UserEntity user, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}