using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TodoWeb.Dtos;
using TodoWeb.Models;
using TodoWeb.Data.Providers;
using TodoWeb.Extensions;

namespace TodoWeb.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly ApplicationDbContext _dbContext;
        private readonly CommandResult _commandResult;
        public AccountService(ApplicationDbContext context, IAuthenticationProvider authenticationProvider)
        {
            _dbContext = context;
            _authenticationProvider = authenticationProvider;
            _commandResult = new();
        }
        public async Task<CommandResult> Login(LoginArgs args)
        {
            User? user = await GetUserByEmailAsync(args.Email);
            if (user == null || !VerifyPassword(user, args.Password))
            {
                _commandResult.AddError("Email", "Invalid email or password");
                _commandResult.AddError("Password", "Invalid email or password");
                return _commandResult;
            }
            await _authenticationProvider.LoginAsync(args, user.Id);
            return _commandResult;
        }

        public async Task Logout()
        {
            await _authenticationProvider.LogoutAsync();
        }

        public async Task<CommandResult> Register(RegisterArgs args)
        {
            bool duplicate = await _dbContext.Users.AnyAsync(user => user.Email == args.Email);
            if (duplicate)
            {
                _commandResult.AddError("Email", "A user with this email already exists.");
                return _commandResult;
            } 

            if (ValidatePassword(args.Password))
            {
                (string hashedPassword, string salt) = HashString(args.Password);
                User user = new()
                {
                    Email = args.Email,
                    PasswordHash = hashedPassword,
                    Salt = salt
                };
                try
                {
                    await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();
                }
                // Check for more specific errors (e.g. timeout, concurrency, etc.)
                catch
                {
                    _commandResult.AddError("Account", "There was an error in creating your account.");
                }
            }
            return _commandResult;
        }

        public static (string, string) HashString(string data, int iterations = 1000)
        {
            byte[] salt = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return HashString(data, salt, iterations);
        }
        private static (string, string) HashString(string data, string salt, int iterations = 1000)
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
        public bool VerifyPassword(User user, string password)
        {
            return HashString(password, user.Salt).Item1 == user.PasswordHash;
        }
        public bool ValidatePassword(string password)
        {
            bool validPassword = true;
            if (password.Length <= 8 || password.Length >= 21)
            {
                _commandResult.AddError("Password", "Password must have 8 - 20 characters.");
                validPassword = false;
            }
            if (!Regex.Match(password, @"[a-z]").Success)
            {
                _commandResult.AddError("Password", "Password must contain at least 1 lowercase character.");
                validPassword = false;
            }
            if (!Regex.Match(password, @"[A-Z]").Success)
            {
                _commandResult.AddError("Password", "Password must contain at least 1 uppercase character.");
                validPassword = false;
            }
            if (!Regex.Match(password, @"[0-9]").Success)
            {
                _commandResult.AddError("Password", "Password must contain at least 1 number.");
                validPassword = false;
            }
            return validPassword;
        }
        public async Task<User?> GetCurrentUser()
        {
            string idString = _authenticationProvider
                .GetCurrentUserClaim(ClaimTypes.NameIdentifier) ?? "-1";
            int id = int.Parse(idString);

            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
