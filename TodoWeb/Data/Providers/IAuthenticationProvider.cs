using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Data.Providers
{
    public interface IAuthenticationProvider
    {
        public Task LoginAsync(LoginArgs args, int id, string[]? roles);
        public Task LogoutAsync();
        public string GetCurrentUserClaim(string claimType); 
    }
}
