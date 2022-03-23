using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    [Index(nameof(Email), IsUnique = true, Name = $"{nameof(Email)}Index")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<TodoList> TodoLists { get; set; }
        public virtual ICollection<CoauthorUserTodoList> CoauthoredLists { get; set; }
    }
}
