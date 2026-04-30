using Moq;
using Projects_Management_Studio.App.DTOs.Projects;
using Projects_Management_Studio.App.DTOs.Tasks;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.App.Services;
using Projects_Management_Studio.Domain.Enums;
using Xunit;

namespace Tests.Services
{
    public class DashboardServiceTest
    {



        [Fact]
        public async Task GetDashboardAsync_ReturnsCorrectDashboardData()
        {   
            // arrange
            // arrange
            // arrange
            var projectServiceMock = new Mock<IProjectService>();

            var taskServiceMock = new Mock<ITaskService>();

            var tasks = new List<GetTaskByUserDto>
            {
                new GetTaskByUserDto
                (
                    Guid.NewGuid(),
                    "Task 1",
                    null,
                    Guid.NewGuid(),
                    "project 1",
                    TaskItemStatus.ToDo.ToString()
                ),

                new GetTaskByUserDto
                (
                    Guid.NewGuid(),
                    "Task 2",
                    null,
                    Guid.NewGuid(),
                    "project 1",
                    TaskItemStatus.ToDo.ToString()

                )
            };



            var projects = new List<GetProjectDto>
            {
                new GetProjectDto
                (
                    Guid.NewGuid(),
                    "Project 1",
                    null,
                    true,
                    5
                )
            };



            // act
            // act
            // act
            projectServiceMock.Setup(x => x.GetMyProjectsAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(projects);

            taskServiceMock.Setup(x => x.GetUserTasksAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(tasks);


            var dashboardService = new DashboardService(
                taskServiceMock.Object,
                projectServiceMock.Object
                );

            var result = await dashboardService
                    .GetDashboardAsync(Guid.NewGuid());



            // assert
            // assert 
            // assert 
            Assert.NotNull(result);

            Assert.Single(result.MyProjects);

            Assert.Equal(2, result.TotalTasks);

            Assert.Equal(0, result.CompletedTasks);

            Assert.Equal(2, result.PendingTasks);
        }
    }
}