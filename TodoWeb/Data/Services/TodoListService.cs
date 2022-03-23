using System.Text.RegularExpressions;
using TodoWeb.Dtos;
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
            if (!ValidateTodoList(args))
            {
                return _commandResult;
            }
            User? user = await _accountService.GetCurrentUser();
            if (user == null)
            {
                _commandResult.AddError("User", "Please log in.");
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

        private bool ValidateTodoList(CreateTodoListArgs args)
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
