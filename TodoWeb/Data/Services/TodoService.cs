using AutoMapper;
using System.Text.RegularExpressions;
using System.Data;
using TodoWeb.Dtos;
using TodoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoWeb.Data.Services
{
    public class TodoService : ITodoService
    {
        private readonly CommandResult _commandResult;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly int titleCharLimit = 50;
        private readonly int descriptionCharLimit = 300;
        public TodoService (ApplicationDbContext context, IMapper mapper)
        {
            _commandResult = new CommandResult();
            _mapper = mapper;
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
                    if (await GetByTitleAsync(args.Title) != null)
                    {
                        _commandResult.AddError("Title", "A task with this title already exists.");
                    }
                    else
                    {
                        await _context.Todos.AddAsync(_mapper.Map<Todo>(args));
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

        public async Task<IEnumerable<TodoViewModel>> GetAllAsync()
        {
            List<TodoViewModel> todoViewModels = new();
            IEnumerable<Todo> todos = await _context.Todos.ToListAsync();
            foreach(Todo todo in todos)
            {
                todoViewModels.Add(_mapper.Map<TodoViewModel>(todo));
            }
            return todoViewModels;
        }

        public async Task<TodoViewModel> GetByIdAsync(int id)
        {
            Todo? todo = await _context.Todos.FindAsync(id);
            TodoViewModel todoViewModel = _mapper.Map<TodoViewModel>(todo);
            return todoViewModel;
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

        public async Task<CommandResult> UpdateAsync(int id, UpdateTodoArgs args)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo != null)
            {
                args.Title = args.Title.Trim();
                if (args.Description != null) {
                    args.Description = args.Description.Trim();
                }
                try
                {
                    if (ValidateTodo(_mapper.Map<CreateTodoArgs>(args)))
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
            } else
            {
                _commandResult.AddError("Todos", $"Task with ID {id} not found.");
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
