using BCrypt.Net;

namespace ReviseDotnet.Helpers;

public class HashSalt
{
    public string Hash { get; set; }
    public string Salt { get; set; }
}

public class PasswordHelper
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
    }
}