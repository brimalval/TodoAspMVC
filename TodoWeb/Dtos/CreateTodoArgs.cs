namespace TodoWeb.Dtos
{
    public class CreateTodoArgs
    {
        public string Title { get; set; }
        public string? Description { get; set; } = "";
    }
}
