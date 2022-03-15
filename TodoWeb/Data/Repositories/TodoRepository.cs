using TodoWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TodoWeb.Data.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;
        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Todo todo)
        {
            todo.CreatedDateTime = DateTime.UtcNow;
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Todo todo)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _context.Todos.FirstOrDefaultAsync(todo => todo.Id == id);
        }
        public async Task<Todo?> GetByTitleAsync(string title)
        {
            Todo? todo = await _context.Todos.FirstOrDefaultAsync(todo => todo.Title == title);
            return todo;
        }
        public async Task UpdateAsync(Todo todo)
        {
            _context.Update(todo);
            await _context.SaveChangesAsync();
        }
    }
}
