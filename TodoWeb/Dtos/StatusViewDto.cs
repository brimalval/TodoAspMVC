namespace TodoWeb.Dtos
{
    public class StatusViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual TodoListViewDto TodoList { get; set; }
    }
}
