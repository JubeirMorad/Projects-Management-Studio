using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;

        //
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepo = taskRepository;
        }


        //
        public async Task CreateTaskAsync(string title, string? description, Guid projectId, Guid? AssignedToUserId)
        {
            TaskItem task = new()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                ProjectId = projectId,
                AssignedToUserId = AssignedToUserId
            };

            await _taskRepo.AddAsync(task);
        }

        public async Task<List<TaskItem>?> GetTasksProjectAsync(Guid projectId)
        {
            return await _taskRepo.GetTasksByProjectIdAsync(projectId);
        }


        //
        public async Task<List<TaskItem>?> GetTasksUserAsync(Guid? userId)
        {
            return await _taskRepo.GetTasksByUserIdAsync(userId);
        }

    }
}