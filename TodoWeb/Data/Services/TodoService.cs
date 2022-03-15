using AutoMapper;
using System.Text.RegularExpressions;
using TodoWeb.Data.Repositories;
using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Data.Services
{
    public class TodoService : ITodoService
    {
        private readonly ICommandResult _commandResult;
        private readonly IMapper _mapper;
        private readonly ITodoRepository _todoRepository;
        private readonly int titleCharLimit = 50;
        private readonly int descriptionCharLimit = 300;
        public TodoService (ITodoRepository todoRepository, ICommandResult commandResult, IMapper mapper)
        {
            _commandResult = commandResult;
            _mapper = mapper;
            _todoRepository = todoRepository;
        }
        public async Task<ICommandResult> CreateAsync(CreateTodoArgs args)
        {
            try
            {
                if (await ValidateTodo(args))
                {
                    if (await _todoRepository.GetByTitleAsync(args.Title) != null)
                    {
                        _commandResult.AddError("Title", "A task with this title already exists.");
                    }
                    else
                    {
                        await _todoRepository.CreateAsync(_mapper.Map<Todo>(args));
                    }
                }
            } catch
            {
                _commandResult.AddError("Todo", "There was an error creating your task.");
            }
            return _commandResult;
        }

        public async Task<ICommandResult> DeleteAsync(int id)
        {
            Todo? todo = await _todoRepository.GetByIdAsync(id);
            if (todo != null)
            {
                await _todoRepository.DeleteAsync(todo);
            } else
            {
                _commandResult.AddError("Todo", "The task specified by the ID does not exist.");
            }
            return _commandResult;
        }

        public async Task<IEnumerable<TodoViewModel>> GetAllAsync()
        {
            List<TodoViewModel> todoViewModels = new();
            IEnumerable<Todo> todos = await _todoRepository.GetAllAsync();
            foreach(Todo todo in todos)
            {
                todoViewModels.Add(_mapper.Map<TodoViewModel>(todo));
            }
            return todoViewModels;
        }

        public async Task<TodoViewModel> GetByIdAsync(int id)
        {
            Todo? todo = await _todoRepository.GetByIdAsync(id);
            TodoViewModel todoViewModel = _mapper.Map<TodoViewModel>(todo);
            return todoViewModel;
        }

        public async Task<ICommandResult> ToggleStatus(IEnumerable<int> ids)
        {
            if (ids.Any())
            {
                try
                {
                    var tasks = await _todoRepository.GetAllAsync();
                    var updatedTasks = tasks.Where(task => ids.Contains(task.Id));
                    foreach (var task in updatedTasks)
                    {
                        task.Done = !task.Done;
                        await _todoRepository.UpdateAsync(task);
                    }
                }
                catch
                {
                    _commandResult.AddError("Todos", "Unable to update tasks.");
                }
            }
            return _commandResult;
        }

        public async Task<ICommandResult> UpdateAsync(int id, UpdateTodoArgs args)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo != null)
            {
                try
                {
                    if (await ValidateTodo(_mapper.Map<CreateTodoArgs>(args)))
                    {
                        todo.Description = (args.Description ?? "").Trim();
                        todo.Title = args.Title.Trim();
                        todo.Done = args.Done;
                        await _todoRepository.UpdateAsync(todo);
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

        public async Task<bool> ValidateTodo(CreateTodoArgs args)
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
    }
}
