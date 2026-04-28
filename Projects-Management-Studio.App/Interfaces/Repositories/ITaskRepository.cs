using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.App.DTOs.Tasks;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        void Add(TaskItem taskItem);
        Task<List<GetTaskByUserDto>> GetTasksByUserIdAsync(Guid? userId);
        Task<List<GetTaskByProjectDto>> GetTasksByProjectIdAsync(Guid projectId);
        Task<List<TaskItem>> GetTasksByUserIdAndProjectIdAsync(Guid userId, Guid projectId);
        Task<TaskItem?> GetByIdAsync(Guid taskId);
        void Update(TaskItem taskItem);
        void UpdateRange(IEnumerable<TaskItem> tasks);
    }
}