using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.App.DTOs.Dashboard;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.App.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public DashboardService(ITaskService taskService, IProjectService projectService)
        {
            _projectService = projectService;
            _taskService = taskService;
        }

        public async Task<DashboardDto> GetDashboardAsync(Guid userId)
        {
            var projects = await _projectService
                .GetMyProjectsAsync(userId);

            var tasks = await _taskService
                .GetUserTasksAsync(userId);

            return new DashboardDto
            (
                projects,
                tasks,

                projects.Count,

                tasks.Count,

                tasks.Count(t => t.Status == TaskItemStatus.Done.ToString()),

                tasks.Count(t => t.Status != TaskItemStatus.Done.ToString())
            );
        }
    }
}