using System.ComponentModel;
using TodoWeb.Models;

namespace TodoWeb.Dtos;

public class TodoViewDto : IDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    [DisplayName("Created at")]
    public DateTime CreatedDateTime { get; set; }
    public int CreatedById { get; set; }
    public int TodoListId { get; set; }
    public virtual TodoListViewDto TodoList { get; set; }
    public virtual UserViewDto? CreatedBy { get; set; }
}
