using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.App.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUserRepository _userRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IMemberRepository _memberRepository;

        //
        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IProjectRepository projectRepository, IMemberRepository memberRepository)
        {
            _taskRepo = taskRepository;
            _projectRepo = projectRepository;
            _userRepo = userRepository;
            _memberRepository = memberRepository;
        }


        //
        public async Task CreateTaskAsync(Guid userId, string title, string? description, Guid projectId, Guid? assignedToUserId)
        {

            // check user // alow null
            if (assignedToUserId is not null)
            {
                if (await _userRepo.GetUserByIdAsync(assignedToUserId.Value) is null)
                    throw new Exception("user not found.");
            }

            // check project
            var project = await _projectRepo.GetByIdAsync(projectId)
                                ?? throw new Exception("project not found.");

            if (project.OwnerId != userId)
                throw new Exception("you have no permision to add task here.");

            // check if the assigned user is a member of the project
            if (assignedToUserId is not null)
            {
                bool isMember = await _memberRepository.IsExistAsync(project.Id, assignedToUserId.Value);
                
                if (isMember == false)
                    throw new Exception("user is not a member of the project.");
            }

            TaskItem task = new()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                ProjectId = projectId,
                AssignedToUserId = assignedToUserId
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
                                ?? throw new Exception("task not found.");

            var project = await _projectRepo.GetByIdAsync(task.ProjectId)
                                ?? throw new Exception("project not found.");

            if (project.OwnerId != userId)
                throw new Exception("you have no permision to assign task here.");
    

            // check if the assigned user is a member of the project
            if (assignedToUserId is not null)
            {
                bool isMember = await _memberRepository.IsExistAsync(project.Id, assignedToUserId.Value);

                if (isMember == false)
                    throw new Exception("user is not a member of the project.");
            }


            task.AssignedToUserId = assignedToUserId;

            await _taskRepo.UpdateAsync(task);
        }


        //
        public async Task UpdateTaskAsync(Guid userId, Guid taskId, string title, string? description)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                                ?? throw new Exception("task not found.");

            var project = await _projectRepo.GetByIdAsync(task.ProjectId)
                                ?? throw new Exception("project not found.");

            if (project.OwnerId != userId)
                throw new Exception("you have no permision to update task here.");

            task.Title = title;
            task.Description = description;

            await _taskRepo.UpdateAsync(task);
        }


        //
        public async Task UpdateTaskStatusAsync(Guid userId, Guid taskId, TaskItemStatus status)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);

            if (task is null)
                throw new Exception("task not found.");

            if (task.AssignedToUserId != userId)
                throw new Exception("Unauthorized.");

            task.Status = status;

            await _taskRepo.UpdateAsync(task);
        }
    }
}