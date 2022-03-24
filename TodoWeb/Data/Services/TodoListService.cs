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
        private readonly ITodoService _todoService;
        private readonly CommandResult _commandResult;
        private readonly IAccountService _accountService;
        private readonly int titleCharLimit = 50;
        private readonly int descriptionCharLimit = 300;
        public TodoListService(ApplicationDbContext context,
            IAccountService accountService, 
            ITodoService todoService)
        {
            _context = context;
            _commandResult = new();
            _accountService = accountService;
            _todoService = todoService;
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
            TodoList? todoList = await _context.TodoLists
                .Include(tl => tl.Todos)
                .Include(tl => tl.CoauthorUsers)
                .FirstOrDefaultAsync(tl => tl.Id == id);
            if (todoList != null)
            {
                foreach (var coauthorship in todoList.CoauthorUsers)
                {
                    _context.TodoListCoauthorships.Remove(coauthorship);
                }
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
                .Include(list => list.CoauthorUsers)
                .Where(list => list.CreatedBy == currentUser)
                .ToListAsync();

            return todoLists.Select(tl => tl.GetViewDto());
        }

        public async Task<IEnumerable<TodoListViewDto>> GetUserCoauthoredLists()
        {
            User? user = await _accountService.GetCurrentUser();
            if (user == null)
            {
                return new List<TodoListViewDto>();
            }
            return user.CoauthoredLists
                .Where(coauthorship => coauthorship.ListId != null)
                .Select(coauthorship => {
                    return _context.TodoLists
                    .Include(tl => tl.CreatedBy)
                    .First(tl => tl.Id == coauthorship.ListId)
                    .GetViewDto();
                });
        }

        public async Task<TodoListViewDto?> GetByIdAsync(int id)
        {
            var todoList = await _context.TodoLists
                .Include(list => list.CreatedBy)
                .Include(list => list.Todos)
                .FirstOrDefaultAsync(list => list.Id == id);
            return todoList?.GetViewDto();
        }
        public async Task<CommandResult> UpdateAsync(UpdateTodoListArgs args)
        {
            var todoList = await _context.TodoLists
                .Include(tl => tl.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == args.Id);
            if (todoList == null)
            {
                _commandResult.AddError("TodoLists", $"List with ID {args.Id} not found.");
                return _commandResult;
            } 
            
            args.Title = args.Title.Trim();
            args.Description = args.Description?.Trim();

            if (await HasDuplicateByTitleAsync(args.Title, todoList.CreatedBy))
            {
                _commandResult.AddError("Title", "A task with this title already exists.");
                return _commandResult;
            }                     
            try
            {
                CreateTodoListArgs createArgs = new()
                {
                    Title = args.Title,
                    Description = args.Description
                };
                if (ValidateCreateArgs(createArgs))
                {
                    todoList.Description = (args.Description ?? "").Trim();
                    todoList.Title = args.Title.Trim();
                    _context.TodoLists.Update(todoList);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                _commandResult.AddError("Todos", "There was an error creating your task.");
            }
            return _commandResult;
        }
        public async Task<bool> HasDuplicateByTitleAsync(string title, User? creator = null)
        {
            if (creator == null)
            {
                creator = await _accountService.GetCurrentUser();
            }
            return await _context.TodoLists
                .Include(list => list.CreatedBy)
                .Include(list => list.Todos)
                .Where(list => list.CreatedBy == creator)
                .AnyAsync(list => list.Title == title);
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
