using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.API.Contracts.Tasks
{
    public record AssignTaskRequest
    (
        Guid TaskId,
        Guid? AssignedToUserId
    );
}