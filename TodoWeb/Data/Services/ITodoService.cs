using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoService
    {
        bool ValidateTodo(CreateTodoArgs args);
        Task<IEnumerable<TodoViewModel>> GetAllAsync();
        Task<TodoViewModel> GetByIdAsync(int id);
        Task<CommandResult> CreateAsync(CreateTodoArgs args);
        Task<CommandResult> UpdateAsync(int id, UpdateTodoArgs args);
        Task<CommandResult> DeleteAsync(int id);
        Task<CommandResult> ToggleStatus(IEnumerable<int> ids);
    }
}
