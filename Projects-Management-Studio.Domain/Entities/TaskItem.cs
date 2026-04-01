
using Projects_Management_Studio.Domain.Enums;

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
        public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}