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



        //
        //
        //

        [Fact]
        public async Task DeleteMemberAsync_UserCannotDeleteHimself()
        {
            // Arrange 

            var memberRepo = new Mock<IMemberRepository>();
            var projectRepo = new Mock<IProjectRepository>();
            var userRepo = new Mock<IUserRepository>();
            var taskRepo = new Mock<ITaskRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            Guid currentUserId = Guid.NewGuid();
            Guid projectId = Guid.NewGuid();

            Project project = new Project { Id = projectId, OwnerId = currentUserId };
            
            ProjectMember memberToDelete = new ProjectMember
            {
                Id = Guid.NewGuid(),
                UserId = currentUserId,
                ProjectId = projectId,
                Role = ProjectRole.Member
            };

            List<TaskItem> tasks = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Title = "title 1" , AssignedToUserId = currentUserId, ProjectId = projectId },
                new TaskItem { Id = Guid.NewGuid(), Title = "title 2", AssignedToUserId = currentUserId, ProjectId = projectId }
            };  

            User user = new User { Id = currentUserId, Username = "Current User" };


            //
            //
            // Setup mocks

            memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(currentUserId, projectId)).ReturnsAsync(memberToDelete);
            
            projectRepo.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);

            userRepo.Setup(repo => repo.GetUserByIdAsync(currentUserId)).ReturnsAsync(user);

            taskRepo.Setup(repo => repo.GetTasksByUserIdAndProjectIdAsync(currentUserId, projectId)).ReturnsAsync(tasks);

            var memberService = new MemberService(
                memberRepo.Object,
                projectRepo.Object,
                userRepo.Object,
                unitOfWork.Object,
                taskRepo.Object
            );


            var exception = await Assert.ThrowsAsync<Exception>(() =>
                                        memberService.DeleteMemberAsync(currentUserId, currentUserId, projectId));


            //
            // Assert
            Assert.Equal("You cannot remove yourself from the project.",exception.Message);


            memberRepo.Verify(x => x.Delete(It.IsAny<ProjectMember>()),Times.Never);
        }

        
        //
        //

        [Fact]
        public async Task DeleteMemberAsync_NonOwnerCannotDeleteMember()
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
    
                Project project = new Project { Id = projectId, OwnerId = Guid.NewGuid() };
    
                ProjectMember memberToDelete = new ProjectMember
                {
                    Id = Guid.NewGuid(),
                    UserId = userIdToDelete,
                    ProjectId = projectId,
                    Role = ProjectRole.Member
                };
                User user = new()
                {
                    Id = userIdToDelete,
                    Username = "User To Delete"
                };

                //
                // setup mocks

                projectRepo.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);
                userRepo.Setup(repo => repo.GetUserByIdAsync(userIdToDelete)).ReturnsAsync(user);
                memberRepo.Setup(repo => repo.GetMemberByUserIdAndProjectIdAsync(userIdToDelete, projectId)).ReturnsAsync(memberToDelete);

                var memberService = new MemberService(
                    memberRepo.Object,
                    projectRepo.Object,
                    userRepo.Object,
                    unitOfWork.Object,
                    taskRepo.Object
                );

                // Act
                Exception exception = await Assert.ThrowsAsync<Exception>(() =>
                                        memberService.DeleteMemberAsync(currentUserId, userIdToDelete, projectId));

                // Assert
                Assert.Equal("Only the project owner can delete members.", exception.Message);
        }
    }
}