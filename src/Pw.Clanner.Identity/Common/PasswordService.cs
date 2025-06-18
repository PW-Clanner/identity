using Microsoft.AspNetCore.Identity;
using Pw.Clanner.Identity.Domain.Entities;

namespace Pw.Clanner.Identity.Common;

public static class UserExtensions
{
    public static void GeneratePasswordHash(this UserEntity user, string password)
    {
        var hasher = new PasswordHasher<UserEntity>();
        var hash = hasher.HashPassword(user, password);
        user.PasswordHash = hash;
    }
}