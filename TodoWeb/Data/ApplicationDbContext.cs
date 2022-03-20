using Microsoft.EntityFrameworkCore;
using TodoWeb.Models;
using TodoWeb.Dtos;

namespace TodoWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
