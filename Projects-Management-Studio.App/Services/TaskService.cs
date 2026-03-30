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
        public async Task CreateTaskAsync(Guid userId, string title, string? description, Guid projectId, Guid? AssignedToUserId)
        {

            // check user // alow null
            if (AssignedToUserId is not null)
            {
                if (await _userRepo.GetUserByIdAsync(AssignedToUserId.Value) is null)
                    throw new Exception("user not found.");
            }

            // check project
            var project = await _projectRepo.GetByIdAsync(projectId)
                                ??    throw new Exception("project not found.");

            if (project.OwnerId != userId)
                throw new Exception("you have no permision to add task here.");

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


        //
        public async Task AssignTaskAsync(Guid userId, Guid taskId, Guid? assignedToUserId)
        {
            // check user // alow null
            if (assignedToUserId is not null)
            {
                if (await _userRepo.GetUserByIdAsync(assignedToUserId.Value) is null)
                    throw new Exception("user not found.");
            }

            var task = await _taskRepo.GetByIdAsync(taskId)
                                ??    throw new Exception("task not found.");

            var project = await _projectRepo.GetByIdAsync(task.ProjectId)
                                ??    throw new Exception("project not found.");

            if (project.OwnerId != userId)
                throw new Exception("you have no permision to assign task here.");

            task.AssignedToUserId = assignedToUserId;

            await _taskRepo.UpdateAsync(task);
        }


        //
        public async Task UpdateTaskAsync(Guid userId, Guid taskId, string title, string? description)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                                ??    throw new Exception("task not found.");

            var project = await _projectRepo.GetByIdAsync(task.ProjectId)
                                ??    throw new Exception("project not found.");

            if (project.OwnerId != userId)
                throw new Exception("you have no permision to update task here.");

            task.Title = title;
            task.Description = description;

            await _taskRepo.UpdateAsync(task);
        }
    }
}