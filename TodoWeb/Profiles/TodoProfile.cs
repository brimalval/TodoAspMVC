using AutoMapper;
using TodoWeb.Dtos;
using TodoWeb.Models;

namespace TodoWeb.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            // Mutation of a todo item converts args into todo model
            CreateMap<CreateTodoArgs, Todo>();
            CreateMap<UpdateTodoArgs, Todo>();
            // CreateTodoArgs is also a subset of UpdateTodoArgs
            CreateMap<UpdateTodoArgs, CreateTodoArgs>();
            CreateMap<UpdateTodoArgs, TodoViewModel>();
            // Viewing of a todo extracts viewable info from todo model
            CreateMap<Todo, TodoViewModel>();
        }
    }
}
