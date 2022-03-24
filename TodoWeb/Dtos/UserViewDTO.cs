using TodoWeb.Models;

namespace TodoWeb.Dtos
{
    public class UserViewDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
