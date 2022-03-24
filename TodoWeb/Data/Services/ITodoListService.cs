using TodoWeb.Dtos;

namespace TodoWeb.Data.Services
{
    public interface ITodoListService :
        ICrudService<TodoListViewDto, CreateTodoListArgs, UpdateTodoListArgs>
    {
        Task<IEnumerable<TodoListViewDto>> GetUserCoauthoredLists();
    }
}
