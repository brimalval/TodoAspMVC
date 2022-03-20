using Microsoft.EntityFrameworkCore;

namespace TodoWeb.Dtos
{
    [Keyless]
    public class CreateTodoArgs
    {
        public string Title { get; set; }
        public string? Description { get; set; } = "";
    }
}
