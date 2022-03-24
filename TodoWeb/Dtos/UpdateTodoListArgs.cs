using TodoWeb.Models;

namespace TodoWeb.Dtos
{
    public class UpdateTodoListArgs : IArgsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
