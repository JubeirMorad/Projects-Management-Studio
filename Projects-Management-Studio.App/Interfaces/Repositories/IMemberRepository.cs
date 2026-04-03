

using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<ProjectMember?> GetByIdAsync(Guid id);
        Task<List<ProjectMember>?> GetByUserIdAsync(Guid userId);
        Task<List<ProjectMember>?> GetByProjectIdAsync(Guid projectId); 
    }
}