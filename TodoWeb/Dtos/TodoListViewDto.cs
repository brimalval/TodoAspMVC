using System.ComponentModel;
using TodoWeb.Models;

namespace TodoWeb.Dtos
{
    public class TodoListViewDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        [DisplayName("Created By")]
        public User CreatedBy { get; set; }
        public ICollection<Todo> Todos { get; set; }
        public ICollection<User> Authors { get; set; }
    }
}
