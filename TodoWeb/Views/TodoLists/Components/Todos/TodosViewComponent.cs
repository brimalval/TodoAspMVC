using Microsoft.AspNetCore.Mvc;
using TodoWeb.Data.Services;
using TodoWeb.Dtos;

namespace TodoWeb.Views.ViewComponents
{
    public class TodosViewComponent : ViewComponent
    {
        private readonly ITodoListService _todoListService;
        public TodosViewComponent(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }
        public async Task<IViewComponentResult> InvokeAsync(
            int listId,
            TodoListViewDto? list,
            int pageNumber=1,
            int pageSize=5)
        {
            list ??= await _todoListService.GetByIdAsync(listId);
            var paginatedTodos = _todoListService.GetTodosPaginated(list, pageNumber, pageSize);
            return View(paginatedTodos);
        }
    }
}
