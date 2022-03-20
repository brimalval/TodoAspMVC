using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TodoWeb.Dtos;
using TodoWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.RegularExpressions;

namespace TodoWeb.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpContext _httpContext;
        private readonly ApplicationDbContext _dbContext;
        private readonly CommandResult _commandResult;
        public AccountService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _dbContext = context;
            _httpContext = contextAccessor.HttpContext!;
            _commandResult = new();
        }
        public async Task<CommandResult> Login(LoginArgs args)
        {
            User? user = await GetUserByEmailAsync(args.Email);
            if (user == null || !VerifyPassword(user, args.Password))
            {
                _commandResult.AddError("Email", "Invalid email or password");
                _commandResult.AddError("Password", " ");
            } 
            else
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Email, args.Email),
                    new Claim(ClaimTypes.NameIdentifier, $"{user.Id}")
                };

                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new(identity);
                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = args.RememberMe
                };

                if (_httpContext != null)
                {
                    await _httpContext.SignInAsync(principal, authProperties);
                } 
                else
                {
                    _commandResult.AddError("User", "Login failed.");
                }
            }
            return _commandResult;
        }

        public async Task Logout()
        {
            if (_httpContext != null)
            {
                await _httpContext.SignOutAsync();
            }
        }

        public async Task<CommandResult> Register(RegisterArgs args)
        {
            bool duplicate = await _dbContext.Users.AnyAsync(user => user.Email == args.Email);
            if (duplicate)
            {
                _commandResult.AddError("Email", "A user with this email already exists.");
            } 
            else if (ValidatePassword(args.Password))
            {
                string hashedPassword = HashString(args.Password);
                User user = new()
                {
                    Email = args.Email,
                    PasswordHash = hashedPassword,
                };
                try
                {
                    await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();
                } catch
                {
                    _commandResult.AddError("Account", "There was an error in creating your account.");
                }
            }
            return _commandResult;
        }

        public string HashString(string data)
        {
            byte[] stringBytes = ASCIIEncoding.ASCII.GetBytes(data);
            byte[] hashedBytes = MD5.Create().ComputeHash(stringBytes);
            StringBuilder stringHash = new();
            foreach (var hashedByte in hashedBytes)
            {
                stringHash.Append(hashedByte.ToString("x2"));
            }
            return stringHash.ToString();
        }

        public bool VerifyPassword(User user, string password)
        {
            return HashString(password) == user.PasswordHash;
        }
        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            User user = await _dbContext.Users.FirstAsync(u => u.Email == email);
            return HashString(password) == user.PasswordHash;
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
            string idString = _httpContext.User
                .FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?
                .Value ?? "-1";
            int id = int.Parse(idString);

            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
