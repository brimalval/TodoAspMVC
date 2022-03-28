using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoListService :
        ICrudService<TodoListViewDto, CreateTodoListArgs, UpdateTodoListArgs>
    {
        Task<CommandResult> AddPermission(int id, int coauthorId);
        Task<CommandResult> RemovePermission(int id, int coauthorId);
        Task<IEnumerable<UserViewDto>> GetNonCoauthors(int v);
    }
}
