using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoWeb.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        public virtual User? CreatedBy { get; set; }
        public int TodoListId { get; set; }
        public virtual TodoList TodoList { get; set; }
        public int? StatusId { get; set; }
        public virtual Status? Status { get; set; }
    }
}
