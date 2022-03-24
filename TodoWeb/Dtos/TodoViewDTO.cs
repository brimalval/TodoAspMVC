using TodoWeb.Models;

namespace TodoWeb.Dtos
{
    public class TodoViewDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public int TodoListId { get; set; }
        public virtual TodoList TodoList { get; set; }
        public virtual User? CreatedBy { get; set; }
    }
}
