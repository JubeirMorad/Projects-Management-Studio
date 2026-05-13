using Projects_Management_Studio.App.DTOs.Tasks;
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
        private readonly IUnitOfWork _unitOfWork;

        //
        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IProjectRepository projectRepository, IMemberRepository memberRepository, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepository;
            _projectRepo = projectRepository;
            _userRepo = userRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
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
                
                if (!isMember)
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

            _taskRepo.Add(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GetTaskByProjectDto>> GetProjectTasksAsync(Guid currentUserId, Guid projectId)
        {
            Project? project = await _projectRepo.GetByIdAsync(projectId);

            if (project is null)
                throw new Exception("project not fount.");

            if (currentUserId != project.OwnerId)
            {
                ProjectMember? member = await _memberRepository.GetMemberByUserIdAndProjectIdAsync(currentUserId, projectId);

                if (member is null || member.Role != ProjectRole.Admin)
                    throw new Exception("you do not have access to the tasks in this project.");
            }

            return await _taskRepo.GetTasksByProjectIdAsync(projectId);
        }


        //
        public async Task<List<GetTaskByUserDto>> GetUserTasksAsync(Guid? userId)
        {
            return await _taskRepo.GetTasksByUserIdAsync(userId);
        }


        //
        public async Task AssignTaskAsync(Guid currentUserId, Guid taskId, Guid? assignedToUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId)
                                ?? throw new Exception("task not found.");

            var project = await _projectRepo.GetByIdAsync(task.ProjectId)
                                ?? throw new Exception("project not found.");

            if (currentUserId == assignedToUserId)
                throw new Exception("cannot assign task for you.");

            if (project.OwnerId != currentUserId)
            {
                ProjectMember? currentMember = await _memberRepository.GetMemberByUserIdAndProjectIdAsync(currentUserId, task.ProjectId);

                if (currentMember is null || currentMember.Role != ProjectRole.Admin)
                    throw new Exception("you have no permission to assign task here.");
                
            }

            // check if the assigned user is a member of the project
            if (assignedToUserId is not null)
            {
                ProjectMember? member = await _memberRepository.GetMemberByUserIdAndProjectIdAsync(assignedToUserId.Value, task.ProjectId);

                if (member is null)
                    throw new Exception("user is not a member of the project.");

                if (project.OwnerId != currentUserId && member.Role == ProjectRole.Admin)
                    throw new Exception("only owner can assign task for admin.");

            }


            task.AssignedToUserId = assignedToUserId;

            _taskRepo.Update(task);
            await _unitOfWork.SaveChangesAsync();
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

            _taskRepo.Update(task);
            await _unitOfWork.SaveChangesAsync();
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

            _taskRepo.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}