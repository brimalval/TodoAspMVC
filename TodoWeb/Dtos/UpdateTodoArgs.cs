namespace TodoWeb.Dtos
{
    public class UpdateTodoArgs : IArgsDto
    {
        public UpdateTodoArgs() {}
        public UpdateTodoArgs(TodoViewDto todoViewDto)
        {
            Description = todoViewDto.Description;
            Id = todoViewDto.Id;
            Title = todoViewDto.Title;
            TodoListId = todoViewDto.TodoListId;
            Status = todoViewDto.Status;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = "";
        public int TodoListId { get; set; }
        public int? StatusId { get; set; }
        public StatusViewDto? Status { get; set; }
    }
}
