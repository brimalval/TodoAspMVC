namespace TodoWeb.Dtos
{
    public class CreateTodoArgs : IDto
    {
        public string Title { get; set; }
        public string? Description { get; set; } = "";
    }
}
