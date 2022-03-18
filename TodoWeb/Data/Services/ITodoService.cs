using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Data.Services
{
    public interface ITodoService
    {
        bool ValidateTodo(CreateTodoArgs args);
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo?> GetByIdAsync(int id);
        Task<CommandResult> CreateAsync(CreateTodoArgs args);
        Task<CommandResult> UpdateAsync(UpdateTodoArgs args);
        Task<CommandResult> DeleteAsync(int id);
        Task<CommandResult> ToggleStatus(IEnumerable<int> ids);
    }
}
