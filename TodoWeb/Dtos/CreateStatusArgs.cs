using System.ComponentModel.DataAnnotations;

namespace TodoWeb.Dtos
{
    public class CreateStatusArgs : IArgsDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\-_\s()]+")]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int ListId { get; set; }
    }
}
