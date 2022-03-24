using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoListService
    {
        public Task<IEnumerable<TodoListViewDto>> GetAllAsync();
        public Task<TodoListViewDto?> GetByIdAsync(int id);
        public Task<CommandResult> CreateAsync(CreateTodoListArgs args);
    }
}
