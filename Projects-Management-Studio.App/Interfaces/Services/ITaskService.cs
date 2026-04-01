using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.App.Interfaces.Services
{
    public interface ITaskService
    {
        Task CreateTaskAsync(Guid userId, string title, string? description, Guid projectId, Guid? AssignedToUserId);
        Task<List<TaskItem>?> GetTasksUserAsync(Guid? userId);
        Task<List<TaskItem>?> GetTasksProjectAsync(Guid projectId);

        Task AssignTaskAsync(Guid userId, Guid taskId, Guid? assignedToUserId);
        Task UpdateTaskAsync(Guid userId, Guid taskId, string title, string? description);

        Task UpdateTaskStatusAsync(Guid userId, Guid taskId, TaskItemStatus status);
    }
}