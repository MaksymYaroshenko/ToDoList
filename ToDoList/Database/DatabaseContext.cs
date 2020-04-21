using Microsoft.EntityFrameworkCore;
using ToDoList.Database.Entities;

namespace ToDoList.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.AppSetting["Database:ConnectionString"]);
        }
    }
}
