using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TodoWeb.Dtos;

namespace TodoWeb.Data.Providers
{
    public class CookieAuthenticationProvider : IAuthenticationProvider
    {
        private readonly HttpContext _httpContext;
        public CookieAuthenticationProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public string GetCurrentUserClaim(string claimType)
        {
            return _httpContext.User.FindFirstValue(claimType);
        }

        public async Task LoginAsync(LoginArgs args, int id)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email, args.Email),
                new Claim(ClaimTypes.NameIdentifier, $"{id}")
            };

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = args.RememberMe
            };

            await _httpContext.SignInAsync(principal, authProperties);
        }

        public async Task LogoutAsync()
        {
            await _httpContext.SignOutAsync();
        }
    }
}
