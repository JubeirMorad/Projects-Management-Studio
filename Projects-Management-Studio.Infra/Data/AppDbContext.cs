
using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.Infra.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; } 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}