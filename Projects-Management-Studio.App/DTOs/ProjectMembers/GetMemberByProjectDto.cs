using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.App.DTOs.ProjectMembers
{
    public record GetMemberByProjectDto
    (
        Guid UserId,
        string UserName,
        string Role,
        bool isCurrentUser
    );
}