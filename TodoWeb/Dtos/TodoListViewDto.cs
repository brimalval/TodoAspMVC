using TodoWeb.Models;

namespace TodoWeb.Dtos
{
    public class TodoListViewDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<Todo> Todos { get; set; }
    }
}
