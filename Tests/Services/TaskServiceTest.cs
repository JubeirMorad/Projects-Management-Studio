

using Moq;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Services;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Domain.Enums;

namespace Tests.Services
{
    public class TaskServiceTest
    {
        [Fact]
        public async Task AssignTaskAsync_OwnerCanAssignTask()
        {
            //
            //
            // Arrange 
            var userRepo = new Mock<IUserRepository>();
            var projectRepo = new Mock<IProjectRepository>();
            var taskRepo = new Mock<ITaskRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            Guid taskId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Guid projectId = Guid.NewGuid();
            Guid currentUserId = Guid.NewGuid();


            TaskItem task = new()
            {
                Id = taskId,
                Title = "task 1",
                Description = null,
                AssignedToUserId = null,
                ProjectId = projectId
            };

            Project project = new()
            {
                Id = projectId,
                Name = "Project 1",
                Description = null,
                OwnerId = currentUserId
            };

            //
            // setup mocks

            taskRepo.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            projectRepo.Setup(repo => repo.GetByIdAsync(task.ProjectId)).ReturnsAsync(project);

            memberRepo.Setup(repo => repo.IsExistAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var taskService = new TaskService(
                taskRepo.Object,
                userRepo.Object,
                projectRepo.Object,
                memberRepo.Object,
                unitOfWork.Object
            );

            //
            //
            // act
            await taskService.AssignTaskAsync(currentUserId, taskId, userId);


            //
            // Assert
            taskRepo.Verify(repo => repo.Update(task), Times.Once);
            unitOfWork.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.Equal(userId, task.AssignedToUserId);
        }
    

        [Fact]
        public async Task AssignTaskAsync_AdminCanAssignTask()
        {
            //
            //
            // Arrange 
            var userRepo = new Mock<IUserRepository>();
            var projectRepo = new Mock<IProjectRepository>();
            var taskRepo = new Mock<ITaskRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            Guid taskId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            Guid projectId = Guid.NewGuid();
            Guid currentUserId = Guid.NewGuid();

            TaskItem task = new()
            {
                Id = taskId,
                Title = "task 1",
                Description = null,
                AssignedToUserId = Guid.NewGuid(),
                ProjectId = projectId
            };

            Project project = new()
            {
                Id = projectId,
                Name = "Project 1",
                Description = null,
                OwnerId = Guid.NewGuid()
            };

            ProjectMember adminMember = new()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                UserId = currentUserId,
                Role = ProjectRole.Admin
            };

            //
            //
            // setup mocks
            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(currentUserId, projectId)).ReturnsAsync(adminMember);
            
            taskRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(task);

            projectRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(project);

            memberRepo.Setup(repo => repo.IsExistAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var memberService = new TaskService(
                taskRepo.Object,
                userRepo.Object,
                projectRepo.Object,
                memberRepo.Object,
                unitOfWork.Object
            );

            //
            //
            // act
            await memberService.AssignTaskAsync(currentUserId, taskId, userId);

            //
            // Assert
            taskRepo.Verify(repo => repo.Update(task), Times.Once);
            unitOfWork.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.Equal(userId, task.AssignedToUserId);
        }
    
        
    }
}