using ECommerce.Application.Interfaces;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Auth;

public class PasswordService : IPasswordService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100000;

    public (string Hash, string Salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] saltBytes = new byte[SaltSize];
        rng.GetBytes(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
        byte[] hashBytes = pbkdf2.GetBytes(KeySize);

        return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] hashBytes = Convert.FromBase64String(hash);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
        byte[] computedHash = pbkdf2.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(hashBytes, computedHash);
    }
}