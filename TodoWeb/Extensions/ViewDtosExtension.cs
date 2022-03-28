using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Extensions
{
    public static class ViewDtosExtension
    {
        public static TodoViewDto GetViewDto(this Todo todo) => new()
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Done = todo.Done,
            CreatedDateTime = todo.CreatedDateTime,
            TodoList = todo.TodoList,
            CreatedBy = todo.CreatedBy,
        };
        public static TodoListViewDto GetViewDto(this TodoList todoList) => new()
        {
            Id = todoList.Id,
            Todos = todoList.Todos,
            Title = todoList.Title,
            Description = todoList.Description,
            Authors = todoList.Authors
        };
        public static UserViewDto GetViewDto(this User user) => new()
        {
            Email = user.Email,
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            Roles = user.Roles,
            Salt = user.Salt
        };
    }
}
