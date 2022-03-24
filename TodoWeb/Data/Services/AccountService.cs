using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoWeb.Dtos;
using TodoWeb.Models;
using TodoWeb.Data.Providers;
using static TodoWeb.Utilities.PasswordUtilities;

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
            if (user == null || !VerifyPassword(user.PasswordHash, user.Salt, args.Password))
            {
                _commandResult.AddError("Email", "Invalid email or password");
                _commandResult.AddError("Password", "Invalid email or password");
                return _commandResult;
            }
            string[] roleStrings = user.Roles.Select(x => x.Name).ToArray();
            await _authenticationProvider.LoginAsync(args, user.Id, roleStrings);
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

            if (ValidatePassword(args.Password, _commandResult))
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
        public async Task<User?> GetCurrentUser()
        {
            string idString = _authenticationProvider
                .GetCurrentUserClaim(ClaimTypes.NameIdentifier) ?? "-1";
            int id = int.Parse(idString);

            return await _dbContext.Users
                .Include(user => user.CoauthoredLists)
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user = await _dbContext.Users
                .Include(user => user.Roles)
                .Include(user => user.CoauthoredLists)
                .FirstOrDefaultAsync(user => user.Email == email);
            return user;
        }
    }
}
