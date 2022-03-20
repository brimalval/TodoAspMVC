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
        public bool Done { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        [Required]
        public User CreatedBy { get; set; }
    }
}
