namespace TodoWeb.Dtos
{
    public class RoleViewDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<UserViewDto> UsersInRole { get; set; }
    }
}
