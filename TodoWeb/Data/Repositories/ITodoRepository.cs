using TodoWeb.Models;

namespace TodoWeb.Data.Repositories
{
    public interface ITodoRepository
    {
        public Task<IEnumerable<Todo>> GetAllAsync();
        public Task<Todo?> GetByIdAsync(int id);
        public Task<Todo?> GetByTitleAsync(string title);
        public Task CreateAsync(Todo todo);
        public Task UpdateAsync(Todo todo);
        public Task DeleteAsync(Todo todo);
    }
}
