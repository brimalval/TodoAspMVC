namespace TodoWeb.Dtos
{
    public class CreateTodoArgs : IArgsDto
    {
        public string Title { get; set; }
        public string? Description { get; set; } = "";
        public int ListId { get; set; }
    }
}
