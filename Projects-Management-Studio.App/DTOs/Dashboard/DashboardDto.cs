using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.App.DTOs.Projects;
using Projects_Management_Studio.App.DTOs.Tasks;

namespace Projects_Management_Studio.App.DTOs.Dashboard
{
    public record DashboardDto
    (
        List<GetProjectDto> MyProjects ,
        List<GetTaskByUserDto> MyTasks ,
        int TotalProjects ,
        int TotalTasks ,
        int CompletedTasks ,
        int PendingTasks 
    );
}