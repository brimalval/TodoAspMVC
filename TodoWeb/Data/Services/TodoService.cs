using System.Text.RegularExpressions;
using System.Data;
using TodoWeb.Dtos;
using TodoWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TodoWeb.Data.Services
{
    public class TodoService : ITodoService
    {
        private readonly IAccountService _accountService;
        private readonly CommandResult _commandResult;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpContext _httpContext;
        private readonly int titleCharLimit = 50;
        private readonly int descriptionCharLimit = 300;
        public TodoService (ApplicationDbContext context, 
            IHttpContextAccessor httpContextAccessor,
            IAccountService accountService)
        {
            _accountService = accountService;
            _commandResult = new CommandResult();
            _httpContext = httpContextAccessor.HttpContext!;
            _dbContext = context;
        }
        public async Task<CommandResult> CreateAsync(CreateTodoArgs args)
        {
            User? user = await _accountService.GetCurrentUser();
            if (user == null)
            {
                _commandResult.AddError("User", "User not currently logged in.");
                return _commandResult;
            }
            try
            {
                args.Title = args.Title.Trim();
                args.Description = args.Description?.Trim();
                if (ValidateTodo(args))
                {
                    if (await GetByTitleAsync(args.Title) != null)
                    {
                        _commandResult.AddError("Title", "A task with this title already exists.");
                        return _commandResult;
                    }
                    Todo todo = new()
                    {
                        Title = args.Title,
                        Description = args.Description,
                        CreatedBy = user
                    };
                    await _dbContext.Todos.AddAsync(todo);
                    await _dbContext.SaveChangesAsync();
                }
            } catch
            {
                _commandResult.AddError("Todo", "There was an error creating your task.");
            }
            return _commandResult;
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            Todo? todo = await _dbContext.Todos.FindAsync(id);
            if (todo != null)
            {
                _dbContext.Todos.Remove(todo);
                await _dbContext.SaveChangesAsync(); ;
                return _commandResult;
            } 
            _commandResult.AddError("Todo", "The task specified by the ID does not exist.");
            return _commandResult;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            User? currentUser = await _accountService.GetCurrentUser();
            IEnumerable<Todo> todos = await _dbContext.Todos
                .Where(todo => todo.CreatedBy == currentUser)
                .ToListAsync();
            return todos;
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            User? user = await _accountService.GetCurrentUser();
            Todo? todo = await _dbContext.Todos
                .Where(t => t.CreatedBy == user)
                .FirstOrDefaultAsync(t => t.Id == id);
            return todo;
        }

        public async Task<CommandResult> ToggleStatus(IEnumerable<int> ids)
        {
            if (!ids.Any())
            {
                return _commandResult;
            }

            User? user = await _accountService.GetCurrentUser();
            try
            {
                var tasks = await GetAllAsync();
                var updatedTasks = tasks
                    .Where(task => ids.Contains(task.Id));

                foreach (var task in updatedTasks)
                {
                    task.Done = !task.Done;
                    _dbContext.Todos.Update(task);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _commandResult.AddError("Todos", "Unable to update tasks.");
            }
            return _commandResult;
        }

        public async Task<CommandResult> UpdateAsync(UpdateTodoArgs args)
        {
            var todo = await _dbContext.Todos.FindAsync(args.Id);
            if (todo == null)
            {
                _commandResult.AddError("Todos", $"Task with ID {args.Id} not found.");
                return _commandResult;
            } 
            
            User? user = await _accountService.GetCurrentUser();
            args.Title = args.Title.Trim();
            args.Description = args.Description?.Trim();

            bool duplicateExists = await _dbContext.Todos
                .AnyAsync(t => t.Id != args.Id && t.Title == args.Title && t.CreatedBy == user);
            if (duplicateExists)
            {
                _commandResult.AddError("Title", "A task with this title already exists.");
                return _commandResult;
            }                     
            try
            {
                CreateTodoArgs createArgs = new()
                {
                    Title = args.Title,
                    Description = args.Description
                };
                if (ValidateTodo(createArgs))
                {
                    todo.Description = (args.Description ?? "").Trim();
                    todo.Title = args.Title.Trim();
                    todo.Done = args.Done;
                    _dbContext.Todos.Update(todo);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch
            {
                _commandResult.AddError("Todos", "There was an error creating your task.");
            }
            return _commandResult;
        }

        public bool ValidateTodo(CreateTodoArgs args)
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

        public async Task<Todo?> GetByTitleAsync(string title) 
        {
            User? user = await _accountService.GetCurrentUser();
            return await _dbContext.Todos
                .FirstOrDefaultAsync(todo => todo.Title == title && todo.CreatedBy == user);
        }
    }
}
