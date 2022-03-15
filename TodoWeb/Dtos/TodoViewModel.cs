using System.ComponentModel;

namespace TodoWeb.Dtos
{
    public class TodoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        [DisplayName("Created at")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
