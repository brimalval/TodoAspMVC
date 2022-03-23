using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Data.Services
{
    public interface IAccountService
    {
        public Task<CommandResult> Login(LoginArgs args);
        public Task Logout();
        public Task<CommandResult> Register(RegisterArgs args);
        public Task<User?> GetCurrentUser();
    }
}
