using Microsoft.EntityFrameworkCore;
using TodoWeb.Models;

namespace TodoWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<CoauthorUserTodoList> TodoListCoauthorships { get; set; }
        
        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<TodoList>()
                .HasOne(tl => tl.CreatedBy)
                .WithMany(u => u.TodoLists)
                .HasForeignKey(tl => tl.CreatedById);

            builder.Entity<CoauthorUserTodoList>()
                .HasKey(utl => utl.Id);
            builder.Entity<CoauthorUserTodoList>()
                .HasOne(utl => utl.User)
                .WithMany(u => u.CoauthoredLists)
                .HasForeignKey(utl => utl.UserId);
            builder.Entity<CoauthorUserTodoList>()
                .HasOne(utl => utl.TodoList)
                .WithMany(tl => tl.CoauthorUsers)
                .HasForeignKey(utl => utl.ListId);
        }
    }
}
