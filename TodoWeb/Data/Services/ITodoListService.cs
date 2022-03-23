using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoListService
    {
        public Task<CommandResult> CreateAsync(CreateTodoListArgs args);
    }
}
