using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        //
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        //
        public Guid? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
        //
        public string Status { get; set; } = "ToDo";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}