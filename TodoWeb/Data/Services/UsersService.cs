using Microsoft.EntityFrameworkCore;
using TodoWeb.Dtos;
using TodoWeb.Models;
using static TodoWeb.Utilities.PasswordUtilities;

namespace TodoWeb.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _context;
        private readonly CommandResult _commandResult;
        public UsersService(ApplicationDbContext context)
        {
            _context = context;
            _commandResult = new();
        }
        public async Task<IEnumerable<UserViewDTO>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Roles)
                .ToListAsync();
            var usersDtos = users
                .Select(x => new UserViewDTO
                {
                    Email = x.Email,
                    Id = x.Id,
                    PasswordHash = x.PasswordHash,
                    Roles = x.Roles,
                    Salt = x.Salt
                });
            return usersDtos;
        }

        public async Task<CommandResult> PasswordReset(PasswordResetArgs args)
        {

            User? user = await _context.Users.FindAsync(args.UserId);
            if (user == null)
            {
                _commandResult.AddError("User", "User could not be found.");
                return _commandResult;
            }

            if (ValidatePassword(args.Password, _commandResult))
            {
                (string hash, string salt) = HashString(args.Password);
                user.PasswordHash = hash;
                user.Salt = salt;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return _commandResult;
        }
    }
}
