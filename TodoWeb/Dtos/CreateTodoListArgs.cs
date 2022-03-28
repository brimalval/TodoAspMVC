using Microsoft.EntityFrameworkCore;

namespace TodoWeb.Dtos
{
    [Keyless]
    public class CreateTodoListArgs : IArgsDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
