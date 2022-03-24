﻿using System.Text.RegularExpressions;
using System.Data;
using TodoWeb.Dtos;
using TodoWeb.Models;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Extensions;

namespace TodoWeb.Data.Services
{
    public class TodoService : ITodoService
    {
        private readonly IAccountService _accountService;
        private readonly CommandResult _commandResult;
        private readonly ApplicationDbContext _dbContext;
        private readonly int titleCharLimit = 50;
        private readonly int descriptionCharLimit = 300;
        public TodoService (ApplicationDbContext context, 
            IAccountService accountService)
        {
            _accountService = accountService;
            _commandResult = new CommandResult();
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
                if (ValidateCreateArgs(args))
                {
                    if (await HasDuplicateByTitleAsync(args.ListId, args.Title))
                    {
                        _commandResult.AddError("Title", "A task with this title already exists.");
                        return _commandResult;
                    }
                    Todo todo = new()
                    {
                        Title = args.Title,
                        Description = args.Description,
                        CreatedBy = user,
                        TodoListId = args.ListId
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
                await _dbContext.SaveChangesAsync();
                return _commandResult;
            } 
            _commandResult.AddError("Todo", "The task specified by the ID does not exist.");
            return _commandResult;
        }

        public async Task<IEnumerable<TodoViewDto>> GetAllAsync()
        {
            User? currentUser = await _accountService.GetCurrentUser();
            var todos = await _dbContext.Todos
                .Where(todo => todo.CreatedBy == currentUser)
                .ToListAsync();

            return todos.Select(todo => todo.GetViewDto());
        }

        public async Task<TodoViewDto?> GetByIdAsync(int id)
        {
            Todo? todo = await _dbContext.Todos
                .Include(t => t.TodoList)
                .Include(t => t.CreatedBy)
                .Include(t => t.TodoList)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return null;
            }
            return todo.GetViewDto();
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
                var tasks = _dbContext.Todos;
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
            var todo = await _dbContext.Todos
                .Include(t => t.TodoList)
                .FirstOrDefaultAsync(t => t.Id == args.Id);
            if (todo == null)
            {
                _commandResult.AddError("Todos", $"Task with ID {args.Id} not found.");
                return _commandResult;
            } 
            
            User? user = await _accountService.GetCurrentUser();
            args.Title = args.Title.Trim();
            args.Description = args.Description?.Trim();

            if (await HasDuplicateByTitleAsync(todo.TodoListId, args.Title, todo.Id))
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
                if (ValidateCreateArgs(createArgs))
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
        public bool ValidateCreateArgs(CreateTodoArgs args)
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
        public async Task<bool> HasDuplicateByTitleAsync(int listId, string title, int todoId = -1)
        {
            User? user = await _accountService.GetCurrentUser();

            TodoList? todoList = await _dbContext.TodoLists
                .Include(list => list.Todos)
                .FirstOrDefaultAsync(list => list.Id == listId);

            return todoList?.Todos
                .Any(todo => todo.Title.ToLower() == title.ToLower() && todo.Id != todoId) ?? false;
        }
    }
}
