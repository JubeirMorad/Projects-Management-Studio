using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.API.Contracts.Tasks
{
    public record AddNewTaskRequest
    (
        string Title,
        string? Description,
        Guid ProjectId,
        Guid? AssignedToUserId
    );
}