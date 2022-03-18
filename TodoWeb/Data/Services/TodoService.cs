﻿using System.Text.RegularExpressions;
using System.Data;
using TodoWeb.Dtos;
using TodoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoWeb.Data.Services
{
    public class TodoService : ITodoService
    {
        private readonly CommandResult _commandResult;
        private readonly ApplicationDbContext _context;
        private readonly int titleCharLimit = 50;
        private readonly int descriptionCharLimit = 300;
        public TodoService (ApplicationDbContext context)
        {
            _commandResult = new CommandResult();
            _context = context;
        }
        public async Task<CommandResult> CreateAsync(CreateTodoArgs args)
        {
            try
            {
                args.Title = args.Title.Trim();
                if (args.Description != null) {
                    args.Description = args.Description.Trim();
                }
                if (ValidateTodo(args))
                {
                    // TODO: Change condition to include a check if it comes from the same user
                    if (await GetByTitleAsync(args.Title) != null)
                    {
                        _commandResult.AddError("Title", "A task with this title already exists.");
                    }
                    else
                    {
                        Todo todo = new()
                        {
                            Title = args.Title,
                            Description = args.Description
                        };
                        await _context.Todos.AddAsync(todo);
                        await _context.SaveChangesAsync();
                    }
                }
            } catch
            {
                _commandResult.AddError("Todo", "There was an error creating your task.");
            }
            return _commandResult;
        }

        public async Task<CommandResult> DeleteAsync(int id)
        {
            Todo? todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync(); ;
            } else
            {
                _commandResult.AddError("Todo", "The task specified by the ID does not exist.");
            }
            return _commandResult;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            IEnumerable<Todo> todos = await _context.Todos.ToListAsync();
            return todos;
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            Todo? todo = await _context.Todos.FindAsync(id);
            return todo;
        }

        public async Task<CommandResult> ToggleStatus(IEnumerable<int> ids)
        {
            if (ids.Any())
            {
                try
                {
                    var tasks = await _context.Todos.ToListAsync();
                    var updatedTasks = tasks.Where(task => ids.Contains(task.Id));
                    foreach (var task in updatedTasks)
                    {
                        task.Done = !task.Done;
                        _context.Todos.Update(task);
                    }
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    _commandResult.AddError("Todos", "Unable to update tasks.");
                }
            }
            return _commandResult;
        }

        public async Task<CommandResult> UpdateAsync(UpdateTodoArgs args)
        {
            var todo = await _context.Todos.FindAsync(args.Id);
            if (todo == null)
            {
                _commandResult.AddError("Todos", $"Task with ID {args.Id} not found.");
            } 
            else
            {
                args.Title = args.Title.Trim();
                args.Description = args.Description?.Trim();

                // TODO: Change condition to include a check for if it comes from the same user
                bool duplicateExists = await _context.Todos.AnyAsync(t => t.Id != args.Id && t.Title == args.Title);
                if (duplicateExists)
                {
                    _commandResult.AddError("Title", "A task with this title already exists.");
                } else
                {
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
                            _context.Todos.Update(todo);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch
                    {
                        _commandResult.AddError("Todos", "There was an error creating your task.");
                    }
                }
            } 
            return _commandResult;
        }

        public bool ValidateTodo(CreateTodoArgs args)
        {
            string title = args.Title;
            if (args.Title == null)
            {
                _commandResult.AddError("Title", "Title is required.");
            } 
            else if (title.Trim().Length >=  titleCharLimit)
            {
                _commandResult.AddError("Title", "The inputted title is too long.");
            }
            else if (Regex.Match(title, @"[^a-zA-Z0-9\-_\s()]").Success)
            {
                _commandResult.AddError("Title", "Special characters are not allowed.");
            }

            if (args.Description != null)
            {
                if (args.Description.Length > descriptionCharLimit)
                {
                    _commandResult.AddError("Description", "The given description is too long.");
                }
            }
            return _commandResult.IsValid;
        }

        public async Task<Todo?> GetByTitleAsync(string title) 
        {
            return await _context.Todos.FirstOrDefaultAsync(todo => todo.Title == title);
        }
    }
}
