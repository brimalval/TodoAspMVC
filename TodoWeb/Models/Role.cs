using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<User> UsersInRole { get; set; }
    }
}
