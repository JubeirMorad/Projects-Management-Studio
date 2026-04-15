using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projects_Management_Studio.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; set; }
    
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    
        public string Role { get; set; } = "Member";
    }
}