using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoListService :
        ICrudService<TodoListViewDto, CreateTodoListArgs, UpdateTodoListArgs>
    {
        Task<CommandResult> AddPermission(int id, int coauthorId);
        Task<CommandResult> RemovePermission(int id, int coauthorId);
        Task<CommandResult> SetListStatus(int id, string? status);
        Task<CommandResult> UnsetListStatus(int id);
        Task<CommandResult> PinList(int id);
        Task<CommandResult> ArchiveList(int id);
        Task<IEnumerable<UserViewDto>> GetNonCoauthors(int v);
        Task<IEnumerable<TodoViewDto>?> GetTodosPaginated(int id, int pageNumber, int pageSize);
        IEnumerable<TodoViewDto> GetTodosPaginated(TodoListViewDto todo, int pageNumber, int pageSize);
    }
}
