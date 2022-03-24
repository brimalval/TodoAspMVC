using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ICrudService<T, C, U> 
        where T: class, IDto
        where C: class, IArgsDto
        where U: class, IArgsDto 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<CommandResult> CreateAsync(C args);
        Task<CommandResult> UpdateAsync(U args);
        Task<CommandResult> DeleteAsync(int id);
        bool ValidateCreateArgs(C args);
    }
}
