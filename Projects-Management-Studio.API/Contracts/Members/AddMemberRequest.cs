using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.API.Contracts.Members
{
    public record AddMemberRequest
    (
        Guid ProjectId,
        Guid UserId
    );
}