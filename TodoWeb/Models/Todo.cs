using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"[^a-zA-Z0-9\-_\s()]", 
            ErrorMessage = "Special characters (<,>,{,},etc.) are not allowed.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
