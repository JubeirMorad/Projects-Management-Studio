

using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task AddAsync(ProjectMember member);
        Task<ProjectMember?> GetByIdAsync(Guid id);
        Task<List<ProjectMember>?> GetByUserIdAsync(Guid userId);
        Task<List<ProjectMember>?> GetByProjectIdAsync(Guid projectId);

        Task<ProjectMember?> GetMemberByUserIdAndProjectIdAsync(Guid userId, Guid projectId);

        Task UpdateAsync(ProjectMember member);
    }
}