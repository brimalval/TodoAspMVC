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
        public TodoListService(ApplicationDbContext context,
            IAccountService accountService)
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
            args.Title = args.Title.Trim();

            int duplicates = await CountDuplicatesByTitleAsync(args.Title);
            args.Title += (duplicates > 0 ? $" ({duplicates})" : "");

            TodoList todoList = new()
            {
                Title = args.Title.Trim(),
                Description = args.Description?.Trim(),
            };
            ((ICollection<User>) todoList.Authors).Add(user);
            await _context.TodoLists.AddAsync(todoList);
            await _context.SaveChangesAsync();
            return _commandResult;
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            TodoList? todoList = await _context.TodoLists
                .Include(tl => tl.Todos)
                .Include(tl => tl.Authors)
                .FirstOrDefaultAsync(tl => tl.Id == id);

            if (!await HasPermissionAsync(todoList))
            {
                _commandResult.AddError("User", "User does not have permission to delete this list.");
                return _commandResult;
            }

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
                .Include(list => list.Todos)
                .Include(list => list.Authors)
                .Where(list => list.Authors.Any(author => author == currentUser))
                .ToListAsync();

            return todoLists.Select(tl => tl.GetViewDto());
        }
        public async Task<TodoListViewDto?> GetByIdAsync(int id)
        {
            var todoList = await _context.TodoLists
                .Include(list => list.Todos)
                .Include(list => list.Authors)
                .FirstOrDefaultAsync(list => list.Id == id);

            return await HasPermissionAsync(todoList) 
                ? (todoList?.GetViewDto())
                : null;
        }
        public async Task<bool> HasPermissionAsync(TodoList? todoList)
        {
            User? currentUser = await _accountService.GetCurrentUser();
            if (todoList == null)
            {
                return false;
            }
            if (currentUser != null && 
                currentUser.Roles.Any(role => role.Name == "Admin"))
            {
                return true;
            }

            bool isCoauthor = todoList.Authors
                .Any(c => c.Id == currentUser?.Id);

            return isCoauthor;
        }
        public async Task<CommandResult> UpdateAsync(UpdateTodoListArgs args)
        {
            var todoList = await _context.TodoLists
                .Include(tl => tl.Authors)
                .FirstOrDefaultAsync(t => t.Id == args.Id);
            if (todoList == null)
            {
                _commandResult.AddError("TodoLists", $"List with ID {args.Id} not found.");
                return _commandResult;
            } 

            if (!await HasPermissionAsync(todoList))
            {
                _commandResult.AddError("User", "User does not have permission to update this list.");
                return _commandResult;
            }
            
            args.Title = args.Title.Trim();
            args.Description = args.Description?.Trim();


            int duplicates = await CountDuplicatesByTitleAsync(args.Title);
            args.Title += (duplicates > 0 ? $" ({duplicates})" : "");
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
        public async Task<int> CountDuplicatesByTitleAsync(string title)
        {
            User? creator = await _accountService.GetCurrentUser();
            // TODO: Better duplicate name detection
            return await _context.TodoLists
                .Where(list => list.Authors.Any(coauthor => coauthor == creator))
                .CountAsync(list => list.Title.ToLower().StartsWith(title.ToLower()));
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

        public async Task<IEnumerable<UserViewDto>> GetNonCoauthors(int id)
        {
            TodoList? todoList = await _context.TodoLists
                .Include(tl => tl.Authors)
                .FirstOrDefaultAsync(tl => tl.Id == id);

            if (todoList == null)
            {
                return new List<UserViewDto>();
            }

            var coauthorIds = todoList.Authors
                .Select(coauthor => coauthor.Id);

            var nonCoauthors = await _context.Users
                .Where(user => !coauthorIds.Contains(user.Id))
                .Select(user => user.GetViewDto())
                .ToListAsync();

            return nonCoauthors;
        }

        public async Task<CommandResult> AddPermission(int id, int coauthorId)
        {
            TodoList? todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                _commandResult.AddError("TodoLists", $"Todo list {id} was not found.");
                return _commandResult;
            }
            User? coauthor = await _context.Users.FindAsync(coauthorId);
            if (coauthor == null)
            {
                _commandResult.AddError("Users", $"User {coauthorId} was not found.");
                return _commandResult;
            }

            ((ICollection<User>) todoList.Authors).Add(coauthor);
            await _context.SaveChangesAsync();
            return _commandResult;
        }

        public async Task<CommandResult> RemovePermission(int id, int coauthorId)
        {
            var todoList = await _context.TodoLists
                .FirstOrDefaultAsync(list => list.Id == id
                && list.Authors.Any(author => author.Id == coauthorId));
            if (todoList == null)
            {
                _commandResult.AddError("TodoList", $"Permission for user {coauthorId} on list {id} not found.");
                return _commandResult;
            }
            User? coauthor = await _context.Users.FindAsync(coauthorId);
            if (coauthor == null)
            {
                _commandResult.AddError("Users", $"User {coauthorId} was not found.");
                return _commandResult;
            }
            ((ICollection<User>) todoList.Authors).Remove(coauthor);
            if (!todoList.Authors.Any())
            {
                _context.TodoLists.Remove(todoList);
            }
            await _context.SaveChangesAsync();
            return _commandResult;
        }

        public async Task<IEnumerable<TodoViewDto>?> GetTodosPaginated(int id, int pageNumber, int pageSize)
        {
            var todoList = await GetByIdAsync(id);
            if (todoList == null)
            {
                return null;
            }
            return GetTodosPaginated(todoList, pageNumber, pageSize);
        }

        public IEnumerable<TodoViewDto> GetTodosPaginated(TodoListViewDto todo, int pageNumber, int pageSize)
        {
            return todo.Todos
                .OrderBy(tl => tl.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
