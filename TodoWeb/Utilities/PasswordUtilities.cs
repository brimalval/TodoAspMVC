using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TodoWeb.Data;
using TodoWeb.Extensions;

namespace TodoWeb.Utilities
{
    public class PasswordUtilities
    {
        public static (string, string) HashString(string data, int iterations = 1000)
        {
            byte[] salt = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return HashString(data, salt, iterations);
        }
        public static (string, string) HashString(string data, string salt, int iterations = 1000)
        {
            byte[] saltBytes = salt.ToByteArray();
            return HashString(data, saltBytes, iterations);
        }
        private static (string, string) HashString(string data, byte[] salt, int iterations = 1000)
        {
            using var rfc2898 = new Rfc2898DeriveBytes(data, salt, iterations);
            var hashedBytes = rfc2898.GetBytes(32);
            return (hashedBytes.ToHexString(), salt.ToHexString());
        }
        public static bool VerifyPassword(string userPassword, string salt, string password)
        {
            return HashString(password, salt).Item1 == userPassword;
        }
        public static bool ValidatePassword(string password, CommandResult commandResult)
        {
            bool validPassword = true;
            if (password.Length <= 8 || password.Length >= 21)
            {
                commandResult.AddError("Password", "Password must have 8 - 20 characters.");
                validPassword = false;
            }
            if (!Regex.Match(password, @"[a-z]").Success)
            {
                commandResult.AddError("Password", "Password must contain at least 1 lowercase character.");
                validPassword = false;
            }
            if (!Regex.Match(password, @"[A-Z]").Success)
            {
                commandResult.AddError("Password", "Password must contain at least 1 uppercase character.");
                validPassword = false;
            }
            if (!Regex.Match(password, @"[0-9]").Success)
            {
                commandResult.AddError("Password", "Password must contain at least 1 number.");
                validPassword = false;
            }
            return validPassword;
        }
    }
}
