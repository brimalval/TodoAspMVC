using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoService
    {
        Task<bool> ValidateTodo(CreateTodoArgs args);
        Task<IEnumerable<TodoViewModel>> GetAllAsync();
        Task<TodoViewModel> GetByIdAsync(int id);
        Task<ICommandResult> CreateAsync(CreateTodoArgs args);
        Task<ICommandResult> UpdateAsync(int id, UpdateTodoArgs args);
        Task<ICommandResult> DeleteAsync(int id);
        Task<ICommandResult> ToggleStatus(IEnumerable<int> ids);
    }
}
