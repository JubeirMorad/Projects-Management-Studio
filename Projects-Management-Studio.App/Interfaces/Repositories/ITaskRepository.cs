using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem taskItem);
        Task<List<TaskItem>?> GetTasksByUserIdAsync(Guid? userId);
        Task<List<TaskItem>?> GetTasksByProjectIdAsync(Guid projectId);
    }
}