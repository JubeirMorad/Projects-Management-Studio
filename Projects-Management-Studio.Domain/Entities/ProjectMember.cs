using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; set; }
    
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    
        public ProjectRole Role { get; set; } = ProjectRole.Member;
    }
}