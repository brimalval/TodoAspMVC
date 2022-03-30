using Microsoft.EntityFrameworkCore;
using TodoWeb.Dtos;
using TodoWeb.Extensions;
using TodoWeb.Models;

namespace TodoWeb.Data.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly CommandResult _commandResult;
        private readonly ITodoListService _todoListService;
        private readonly IAccountService _accountService;
        private readonly int maxNameLength = 20;
        public StatusService (
            ApplicationDbContext context,
            ITodoListService todoListService,
            IAccountService accountService)
        {
            _context = context;
            _commandResult = new();
            _todoListService = todoListService;
            _accountService = accountService;
        }
        public async Task<CommandResult> CreateAsync(CreateStatusArgs args)
        {
            var todoList = await _todoListService.GetByIdAsync(args.ListId);
            if (todoList == null)
            {
                _commandResult.AddError("TodoList", $"List with ID {args.ListId} not found.");
                return _commandResult;
            }
            if (!ValidateCreateArgs(args))
            {
                return _commandResult;
            }
            Status status = new()
            {
                Color = args.Color.Trim(),
                Name = args.Name.Trim(),
                TodoListId = args.ListId
            };
            await _context.Statuses.AddAsync(status);
            await _context.SaveChangesAsync();
            return _commandResult;
        }
        public async Task<bool> HasPermissionAsync(Status? status)
        {
            if (status == null)
            {
                return false;
            }

            User? currentUser = await _accountService.GetCurrentUser();
            if (currentUser != null && 
                currentUser.Roles.Any(role => role.Name == "Admin"))
            {
                return true;
            }

            bool isCoauthor = status.TodoList
                .Authors
                .Any(c => c.Id == currentUser?.Id);

            return isCoauthor;
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            Status? status = await _context.Statuses.FindAsync(id);
            if (status == null)
            {
                _commandResult.AddError("Status", $"Status ${id} not found.");
                return _commandResult;
            }

            if (!await HasPermissionAsync(status))
            {
                _commandResult.AddError("Status", "User does not have permission to delete this resource.");
                return _commandResult;
            }
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return _commandResult;
        }

        public Task<IEnumerable<StatusViewDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<StatusViewDto?> GetByIdAsync(int id)
        {
            Status? status = await _context.Statuses
                .Include(s => s.TodoList)
                .FirstOrDefaultAsync(s => s.Id == id);
            return await HasPermissionAsync(status) 
                ? status?.GetViewDto() 
                : null;
        }

        public async Task<CommandResult> UpdateAsync(UpdateStatusArgs args)
        {
            Status? status = await _context.Statuses.FindAsync(args.Id);
            if (status == null)
            {
                _commandResult.AddError("Status", $"Status ${args.Id} not found.");
                return _commandResult;
            }

            if (!await HasPermissionAsync(status))
            {
                _commandResult.AddError("Status", "User does not have permission to delete this resource.");
                return _commandResult;
            }
            var check = new CreateStatusArgs()
            {
                Name = args.Name,
                Color = args.Color
            };
            if (!ValidateCreateArgs(check))
            {
                return _commandResult;
            }
            status.Name = args.Name.Trim();
            status.Color = args.Color.Trim();
            await _context.SaveChangesAsync();
            return _commandResult;
        }

        public bool ValidateCreateArgs(CreateStatusArgs args)
        {
            string name = args.Name.Trim();
            if (string.IsNullOrEmpty(name))
            {
                _commandResult.AddError("Name", "Status name is required.");
            }
            if (name.IsBelowMaxLength(maxNameLength))
            {
                _commandResult.AddError("Name", "The given status name is too long.");
            }
            return _commandResult.IsValid;
        }
    }
}
