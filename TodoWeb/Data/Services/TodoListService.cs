using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TodoWeb.Dtos;
using TodoWeb.Extensions;
using TodoWeb.Models;

namespace TodoWeb.Data.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly ApplicationDbContext _context;
        private readonly CommandResult _commandResult;
        private readonly IAccountService _accountService;
        private readonly int titleCharLimit = 50;
        private readonly int descriptionCharLimit = 300;
        public TodoListService(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _commandResult = new();
            _accountService = accountService;
        }
        public async Task<CommandResult> CreateAsync(CreateTodoListArgs args)
        {
            if (!ValidateCreateArgs(args))
            {
                return _commandResult;
            }
            User? user = await _accountService.GetCurrentUser();
            if (user == null)
            {
                _commandResult.AddError("User", "Please log in.");
                return _commandResult;
            }
            if (await HasDuplicateByTitleAsync(args.Title.Trim()))
            {
                _commandResult.AddError("Title", "You already have a list with this title.");
                return _commandResult;
            }

            TodoList todoList = new()
            {
                CreatedBy = user,
                Title = args.Title.Trim(),
                Description = args.Description?.Trim(),
            };
            await _context.TodoLists.AddAsync(todoList);
            await _context.SaveChangesAsync();
            return _commandResult;
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            TodoList? todoList = await _context.TodoLists.FindAsync(id);
            if (todoList != null)
            {
                _context.TodoLists.Remove(todoList);
                await _context.SaveChangesAsync();
                return _commandResult;
            } 
            _commandResult.AddError("Todo", "The task specified by the ID does not exist.");
            return _commandResult;
        }

        public async Task<IEnumerable<TodoListViewDto>> GetAllAsync()
        {
            User? currentUser = await _accountService.GetCurrentUser();
            var todoLists = await _context.TodoLists
                .Include(list => list.CreatedBy)
                .Include(list => list.Todos)
                .Where(list => list.CreatedBy == currentUser)
                .ToListAsync();

            return todoLists.Select(tl => tl.GetViewDto());
        }

        public async Task<TodoListViewDto?> GetByIdAsync(int id)
        {
            User? currentUser = await _accountService.GetCurrentUser();
            var todoList = await _context.TodoLists
                .Include(list => list.CreatedBy)
                .Include(list => list.Todos)
                .Where(list => list.CreatedBy == currentUser)
                .FirstOrDefaultAsync(list => list.Id == id);
            return todoList?.GetViewDto();
        }
        public async Task<bool> HasDuplicateByTitleAsync(string title)
        {
            User? currentUser = await _accountService.GetCurrentUser();
            return await _context.TodoLists
                .Include(list => list.CreatedBy)
                .Include(list => list.Todos)
                .Where(list => list.CreatedBy == currentUser)
                .AnyAsync(list => list.Title == title);
        }
        public Task<CommandResult> UpdateAsync(UpdateTodoListArgs args)
        {
            throw new NotImplementedException();
        }
        public bool ValidateCreateArgs(CreateTodoListArgs args)
        {
            string title = args.Title;
            if (title.Trim().Length >=  titleCharLimit)
            {
                _commandResult.AddError("Title", "The inputted title is too long.");
            }
            else if (Regex.Match(title, @"[^a-zA-Z0-9\-_\s()]").Success)
            {
                _commandResult.AddError("Title", "Special characters are not allowed.");
            }

            if (args.Description != null && args.Description.Length > descriptionCharLimit)
            {
                _commandResult.AddError("Description", "The given description is too long.");
            }
            return _commandResult.IsValid;
        }
    }
}
