using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.App.DTOs.Projects
{
    public record GetProjectDto
    (
        Guid Id ,
        string Name,
        string? Description,
        bool IsOwner,
        int MemberCount
    );
}