using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public bool Done { get; set; }
        [DisplayName("Created at")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
