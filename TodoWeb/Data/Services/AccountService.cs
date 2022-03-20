using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TodoWeb.Dtos;
using TodoWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TodoWeb.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly CommandResult _commandResult;
        public AccountService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _commandResult = new();
        }
        public async Task<CommandResult> Login(LoginArgs args)
        {
            if (!await VerifyPasswordAsync(args.Email, args.Password))
            {
                _commandResult.AddError("Email", "Invalid email or password");
                _commandResult.AddError("Password", " ");
            } 
            else
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Email, args.Email)
                };
                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new(identity);
                HttpContext? httpContext = _contextAccessor.HttpContext;
                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = args.RememberMe
                };
                if (httpContext != null)
                {
                    await httpContext.SignInAsync(principal, authProperties);
                }
            }
            return _commandResult;
        }

        public async Task Logout()
        {
            HttpContext? httpContext = _contextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignOutAsync();
            }
        }

        public async Task<CommandResult> Register(RegisterArgs args)
        {
            bool duplicate = await _context.Users.AnyAsync(user => user.Email == args.Email);
            if (duplicate)
            {
                _commandResult.AddError("Email", "A user with this email already exists.");
            } 
            else
            {
                string hashedPassword = HashString(args.Password);
                User user = new()
                {
                    Email = args.Email,
                    PasswordHash = hashedPassword,
                };
                try
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
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
            User user = await _context.Users.FirstAsync(u => u.Email == email);
            return HashString(password) == user.PasswordHash;
        }
    }
}
