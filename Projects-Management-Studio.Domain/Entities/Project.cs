using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProjectMember> Members { get; set; } = new HashSet<ProjectMember>();
        public ICollection<TaskItem> Tasks { get; set; } = new HashSet<TaskItem>();
    }
}