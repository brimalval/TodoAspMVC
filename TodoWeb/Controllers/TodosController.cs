#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoWeb.Data;
using TodoWeb.Data.Services;
using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Controllers
{
    [Authorize]
    public class TodosController : ControllerWithErrors
    {
        private readonly ITodoService _todoService;
        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: Todos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _todoService.GetByIdAsync(id ?? -1);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todos/Create
        public IActionResult Create(int? forList)
        {
            if (forList == null)
            {
                return NotFound();
            }
            ViewData["ForList"] = forList;
            return View();
        }

        // POST: Todos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTodoArgs todo)
        {
            var commandResult = await _todoService.CreateAsync(todo);
            if (commandResult.IsValid)
            {
                return RedirectToAction("Details", "TodoLists", new { id = todo.ListId });
            }
            var errors = commandResult.Errors;
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            ViewData["ForList"] = todo.ListId;
            return View(todo);
        }

        // GET: Todos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _todoService.GetByIdAsync(id ?? -1);
            if (todo == null)
            {
                return NotFound();
            }
            var args = new UpdateTodoArgs
            {
                Description = todo.Description,
                Done = todo.Done,
                Id = todo.Id,
                Title = todo.Title,
                TodoListId = todo.TodoList.Id
            };
            return View(args);
        }

        // POST: Todos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTodoArgs args)
        {
            if (!ModelState.IsValid) 
            {
                return View(args);
            }

            var commandResult = await _todoService.UpdateAsync(args);

            return commandResult.IsValid ?
                RedirectToAction("Details", "TodoLists", new { id = args.TodoListId }) :
                ShowErrors(
                    commandResult,
                    args
                );
        }
        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(int? id, int? fromList)
        {
            if (id == null || fromList == null)
            {
                return NotFound();
            }

            var todo = await _todoService.GetByIdAsync(id ?? -1);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int fromList)
        {
            await _todoService.DeleteAsync(id);
            return RedirectToAction("Details", "TodoLists", new { id = fromList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus (List<int> IdList)
        {
            var commandResult = await _todoService.ToggleStatus(IdList);
            if (commandResult.IsValid)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
