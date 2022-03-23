using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class CoauthorUserTodoList
    {
        public int Id;
        public int UserId { get; set; }
        public User User { get; set; }
        public int? ListId { get; set; }
        public TodoList? TodoList { get; set; }
    }
}
