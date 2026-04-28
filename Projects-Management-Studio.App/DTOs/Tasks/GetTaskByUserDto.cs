using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.App.DTOs.Tasks
{
    public record GetTaskByUserDto
    (
        Guid Id,
        string Title,
        string? Description,

        Guid ProjectId,
        string ProjectName,

        string Status
    );
}