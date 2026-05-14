

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

            ProjectMember member = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Role = ProjectRole.Admin,
                ProjectId = projectId
            };
            //
            // setup mocks

            taskRepo.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            projectRepo.Setup(repo => repo.GetByIdAsync(task.ProjectId)).ReturnsAsync(project);

            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(member);

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

            ProjectMember member = new()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                UserId = currentUserId,
                Role = ProjectRole.Member
            };

            //
            //
            // setup mocks
            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(currentUserId, projectId)).ReturnsAsync(adminMember);

            taskRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(task);

            projectRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(project);

            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(userId, It.IsAny<Guid>())).ReturnsAsync(member);

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


        [Fact]
        public async Task AssignTaskAsync_NonAdminNonOwnerCannotAssignTask()
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
                Role = ProjectRole.Member
            };

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
            // act & assert
            Exception exception = await Assert.ThrowsAsync<Exception>(() => memberService.AssignTaskAsync(currentUserId, taskId, userId));

            Assert.Equal("you have no permission to assign task here.", exception.Message);
        }


        [Fact]
        public async Task AssignTaskAsync_CannotAssingTaskToNonMember()
        {
            // Arrange 
            var memberRepo = new Mock<IMemberRepository>();
            var userRepo = new Mock<IUserRepository>();
            var taskRepo = new Mock<ITaskRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var projectRepo = new Mock<IProjectRepository>();

            Guid currentUserId = Guid.NewGuid();
            Guid projectId = Guid.NewGuid();
            Guid taskId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            Project project = new()
            {
                Id = projectId,
                Name = "Project 1",
                Description = null,
                OwnerId = currentUserId
            };

            TaskItem task = new()
            {
                Id = taskId,
                Title = "Task 1",
                AssignedToUserId = Guid.NewGuid(),
                ProjectId = projectId
            };

            //
            //
            // setup mocks
            taskRepo.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            projectRepo.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);



            var memberService = new TaskService(
                taskRepo.Object,
                userRepo.Object,
                projectRepo.Object,
                memberRepo.Object,
                unitOfWork.Object
            );

            //
            // Act
            Exception exception = await Assert.ThrowsAnyAsync<Exception>(() => memberService.AssignTaskAsync(currentUserId, taskId, userId));

            // Assert
            Assert.Equal("user is not a member of the project.", exception.Message);

        }


        [Fact]
        public async Task AssignTaskAsync_AdminCannotAssignTaskForAdmin()
        {
            // Arrange 
            var memberRepo = new Mock<IMemberRepository>();
            var userRepo = new Mock<IUserRepository>();
            var taskRepo = new Mock<ITaskRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var projectRepo = new Mock<IProjectRepository>();

            Guid currentUserId = Guid.NewGuid();
            Guid projectId = Guid.NewGuid();
            Guid taskId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            Project project = new()
            {
                Id = projectId,
                Name = "Project 1",
                Description = null,
                OwnerId = Guid.NewGuid()
            };

            TaskItem task = new()
            {
                Id = taskId,
                Title = "Task 1",
                AssignedToUserId = Guid.NewGuid(),
                ProjectId = projectId
            };

            ProjectMember member = new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Role = ProjectRole.Admin,
                ProjectId = projectId
            };

            ProjectMember currentMember = new()
            {
                Id = Guid.NewGuid(),
                UserId = currentUserId,
                Role = ProjectRole.Admin,
                ProjectId = projectId
            };

            //
            //
            // setup mocks
            taskRepo.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            projectRepo.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);

            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(userId, projectId)).ReturnsAsync(member);

            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(currentUserId, projectId)).ReturnsAsync(currentMember);

            var taskService = new TaskService(
                taskRepo.Object,
                userRepo.Object,
                projectRepo.Object,
                memberRepo.Object,
                unitOfWork.Object
            );

            //
            // Act
            Exception exception = await Assert.ThrowsAnyAsync<Exception>(() => taskService.AssignTaskAsync(currentUserId, taskId, userId));

            // Assert
            Assert.Equal("only owner can assign task for admin.", exception.Message);

        }


        //
        /*** TEST UPDATE TASK STATUS ***/

        [Fact]
        public async Task UpdateTaskStatus_ItWorks()
        {
            //
            // Arrange 

            var memberRepo = new Mock<IMemberRepository>();
            var userRepo = new Mock<IUserRepository>();
            var taskRepo = new Mock<ITaskRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var projectRepo = new Mock<IProjectRepository>();

            Guid currentUserId = Guid.NewGuid();
            Guid taskId = Guid.NewGuid();

            TaskItem task = new()
            {
                Id = taskId,
                Title = "Task 1",
                AssignedToUserId = currentUserId,
                ProjectId = Guid.NewGuid()
            };

            // setup mocks
            taskRepo.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            TaskService taskService = new(
                taskRepo.Object,
                userRepo.Object,
                projectRepo.Object,
                memberRepo.Object,
                unitOfWork.Object
            );

            //
            // Act
            await taskService.UpdateTaskStatusAsync(currentUserId, taskId, TaskItemStatus.Done);

            // Assert
            taskRepo.Verify(repo => repo.GetByIdAsync(taskId), Times.Once);
            unitOfWork.Verify(repo => repo.SaveChangesAsync(), Times.Once);
            taskRepo.Verify(repo => repo.Update(task), Times.Once);
        }


    }
}