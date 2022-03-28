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
            TodoList = todo.TodoList.GetViewDto(),
            CreatedBy = todo.CreatedBy?.GetViewDto(),
        };
        public static TodoListViewDto GetViewDto(this TodoList todoList) => new()
        {
            Id = todoList.Id,
            Todos = todoList.Todos.Select(todo => todo.GetViewDto()),
            Title = todoList.Title,
            Description = todoList.Description,
            Authors = todoList.Authors.Select(author => author.GetViewDto())
        };
        public static UserViewDto GetViewDto(this User user) => new()
        {
            Email = user.Email,
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            Roles = user.Roles.Select(role => role.GetViewDto()),
            Salt = user.Salt
        };
        public static RoleViewDto GetViewDto(this Role role) => new()
        {
            Id = role.Id,
            Name = role.Name,
            UsersInRole = role.UsersInRole.Select(user => user.GetViewDto())
        };
    }
}
