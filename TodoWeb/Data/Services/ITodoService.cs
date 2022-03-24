using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoService : 
        ICrudService<TodoViewDto, CreateTodoArgs, UpdateTodoArgs>
    {
        Task<CommandResult> ToggleStatus(IEnumerable<int> ids);
    }
}
