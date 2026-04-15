
using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.Infra.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                        .HasOne(t => t.AssignedToUser)
                        .WithMany(u => u.Tasks)
                        .HasForeignKey(u => u.AssignedToUserId)
                        .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<ProjectMember>()
                        .HasOne(pm => pm.User)
                        .WithMany(u => u.Members)
                        .HasForeignKey(pm => pm.UserId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ProjectMember>()
                        .HasOne(pm => pm.Project)
                        .WithMany(p => p.Members)
                        .HasForeignKey(pm => pm.ProjectId)
                        .OnDelete(DeleteBehavior.Cascade);
            
            
            modelBuilder.Entity<Project>()
                        .HasOne(p => p.Owner)
                        .WithMany()
                        .HasForeignKey(p => p.OwnerId)
                        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}