using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class TodoList
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public virtual ICollection<Todo> Todos { get; set; }
        public virtual ICollection<CoauthorUserTodoList> CoauthorUsers { get; set; }
    }
}
