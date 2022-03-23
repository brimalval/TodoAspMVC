using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface IUsersService
    {
        public Task<IEnumerable<UserViewDTO>> GetAllAsync();
        public Task<CommandResult> PasswordReset(PasswordResetArgs args); 
    }
}
