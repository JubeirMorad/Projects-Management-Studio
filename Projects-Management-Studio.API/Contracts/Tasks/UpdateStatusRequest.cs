using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.API.Contracts.Tasks
{
    public record UpdateStatusRequest
    (
        TaskItemStatus Status
    );
}