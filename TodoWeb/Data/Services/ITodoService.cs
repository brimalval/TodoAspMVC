using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoService : 
        ICrudService<TodoViewDto, CreateTodoArgs, UpdateTodoArgs>
    {
    }
}
