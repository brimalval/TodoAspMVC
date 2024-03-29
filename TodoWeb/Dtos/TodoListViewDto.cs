﻿namespace TodoWeb.Dtos;

public class TodoListViewDto : IDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? ListState { get; set; }
    public IEnumerable<TodoViewDto> Todos { get; set; }
    public IEnumerable<UserViewDto> Authors { get; set; }
    public virtual IEnumerable<StatusViewDto> Statuses { get; set; }
}
