using Microsoft.EntityFrameworkCore;

namespace TodoWeb.Dtos
{
    [Keyless]
    public class CreateTodoListArgs : IDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CreatedById { get; set; }
    }
}
