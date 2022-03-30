using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int TodoListId { get; set; }
        public virtual TodoList TodoList { get; set; }
    }
}
