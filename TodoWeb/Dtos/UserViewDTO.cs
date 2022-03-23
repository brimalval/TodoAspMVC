using TodoWeb.Models;

namespace TodoWeb.Dtos
{
    public class UserViewDTO
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
