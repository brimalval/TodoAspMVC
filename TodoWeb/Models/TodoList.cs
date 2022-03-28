using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class TodoList
    {
        public TodoList ()
        {
            Authors = new List<User>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Todo> Todos { get; set; }
        public virtual ICollection<User> Authors { get; set; }
    }
}
