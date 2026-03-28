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
        private readonly IUserRepository _userRepo;
        private readonly IProjectRepository _projectRepo;

        //
        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _taskRepo = taskRepository;
            _projectRepo = projectRepository;
            _userRepo = userRepository;
        }


        //
        public async Task CreateTaskAsync(string title, string? description, Guid projectId, Guid? AssignedToUserId)
        {

            // check user // alow null
            if (AssignedToUserId is not null)
            {
                if (await _userRepo.GetUserByIdAsync(AssignedToUserId.Value) is null)
                    throw new Exception("user not found.");
            }

            // check project
            if (await _projectRepo.GetByIdAsync(projectId) is null)
                throw new Exception("project not found.");


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