#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Data.Services;
using TodoWeb.ViewModels;
using TodoWeb.Dtos;

namespace TodoWeb.Controllers
{
    [Authorize]
    public class TodoListsController : ControllerWithErrors
    {
        private readonly ITodoListService _todoListService;

        public TodoListsController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        // GET: TodoLists
        public async Task<IActionResult> Index()
        {
            var lists = await _todoListService.GetAllAsync();
            ListIndexViewModel viewModel = new ListIndexViewModel(lists);
            return View(viewModel);
        }

        // GET: TodoLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoList = await _todoListService.GetByIdAsync(id ?? -1);
            if (todoList == null)
            {
                return NotFound();
            }

            return View(todoList);
        }

        // GET: TodoLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTodoListArgs args)
        {
            var commandResult = await _todoListService.CreateAsync(args);
            return commandResult.IsValid ?
                RedirectToAction("Index") :
                ShowErrors<CreateTodoListArgs>(commandResult, args);
        }

        // GET: TodoLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoList = await _todoListService.GetByIdAsync(id ?? -1);
            if (todoList == null)
            {
                return NotFound();
            }
            var args = new UpdateTodoListArgs
            {
                Id = todoList.Id,
                Title = todoList.Title,
                Description = todoList.Description,
            };
            return View(args);
        }

        // POST: TodoLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTodoListArgs args)
        {
            if (id != args.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _todoListService.UpdateAsync(args);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var todoList = await _todoListService.GetByIdAsync(args.Id);
                    if (todoList != null)
                    {
                        throw;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(args);
        }

        // GET: TodoLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoList = await _todoListService.GetByIdAsync(id ?? -1);
            if (todoList == null)
            {
                return NotFound();
            }

            return View(todoList);
        }

        // POST: TodoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _todoListService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ManagePermissions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TodoListViewDto todoList = await _todoListService.GetByIdAsync(id ?? -1);
            if (todoList == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var nonCoauthors = await _todoListService.GetNonCoauthors(id ?? -1);

            return View((todoList, nonCoauthors));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPermission(int id, int coauthorId)
        {
            var commandResult = await _todoListService.AddPermission(id, coauthorId);
            if (!commandResult.IsValid)
            {
                LoadErrors(commandResult);
            }
            return RedirectToAction(nameof(ManagePermissions), new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePermission(int id, int coauthorId)
        {
            var commandResult = await _todoListService.RemovePermission(id, coauthorId);
            if (!commandResult.IsValid)
            {
                LoadErrors(commandResult);
            }
            return RedirectToAction(nameof(ManagePermissions), new { id });
        }

        public async Task<IActionResult> GetTodos(int id, int pageNumber, int pageSize)
        {
            var list = await _todoListService.GetByIdAsync(id);
            if (list == null)
            {
                return BadRequest($"List with ID {id} not found.");
            }
            return ViewComponent("Todos", new { list, pageNumber, pageSize });
        }
    }
}
