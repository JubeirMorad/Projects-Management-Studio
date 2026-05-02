using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Projects_Management_Studio.App.DTOs.Tasks;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Services;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Domain.Enums;

namespace Tests.Services
{
    public class MemberServiceTest
    {
        [Fact]
        public async Task DeleteMember_OwnerCanDeleteMember()
        {
            // Arrange
            var memberRepo = new Mock<IMemberRepository>();
            var projectRepo = new Mock<IProjectRepository>();
            var userRepo = new Mock<IUserRepository>();
            var taskRepo = new Mock<ITaskRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            Guid currentUserId = Guid.NewGuid();
            Guid userIdToDelete = Guid.NewGuid();
            Guid projectId = Guid.NewGuid();

            Project project = new Project { Id = projectId, OwnerId = currentUserId };

            ProjectMember memberToDelete = new ProjectMember
            {
                Id = Guid.NewGuid(),
                UserId = userIdToDelete,
                ProjectId = projectId,
                Role = ProjectRole.Member
            };


            User userToDelete = new User { Id = userIdToDelete, Username = "User To Delete" };

            List<TaskItem> tasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "title 1" , AssignedToUserId = userIdToDelete, ProjectId = projectId },
                new TaskItem { Id = Guid.NewGuid(), Title = "title 2", AssignedToUserId = userIdToDelete, ProjectId = projectId }
            };

            //
            //
            // Setup mocks

            // get project by id
            projectRepo.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);


            userRepo.Setup(repo => repo.GetUserByIdAsync(userIdToDelete)).ReturnsAsync(userToDelete);


            // get member by user id and project id
            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(userIdToDelete, projectId)).ReturnsAsync(memberToDelete);

            // get tasks by project id
            taskRepo.Setup(repo => repo.GetTasksByUserIdAndProjectIdAsync(userIdToDelete, projectId)).ReturnsAsync(tasks.ToList());

            // create member service
            var memberService = new MemberService(
                memberRepo.Object,
                projectRepo.Object,
                userRepo.Object,
                unitOfWork.Object,
                taskRepo.Object
            );

            //
            //
            // Act
            await memberService.DeleteMemberAsync(currentUserId, userIdToDelete, projectId);

            var tasksAfterDeletion = await taskRepo.Object.GetTasksByProjectIdAsync(projectId);


            //
            //
            // Assert
            memberRepo.Verify(repo => repo.Delete(It.Is<ProjectMember>(m => m.Id == memberToDelete.Id)), Times.Once);

            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);

            Assert.Null(tasks[0].AssignedToUserId);
            Assert.Null(tasks[1].AssignedToUserId);


        }   




    }
}