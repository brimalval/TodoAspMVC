namespace TodoWeb.Dtos
{
    public class UpdateTodoArgs : IArgsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = "";
        public bool Done { get; set; }
        public int TodoListId { get; set; }
    }
}
