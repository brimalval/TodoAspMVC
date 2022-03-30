using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Dtos
{
    public class UpdateStatusArgs : IArgsDto
    {
        public int Id { get; set; } 
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\-_\s()]+")]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
