

using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        void Add(ProjectMember member); 
        Task<ProjectMember?> GetByIdAsync(Guid id);
        Task<List<ProjectMember>> GetByUserIdAsync(Guid userId);
        Task<List<ProjectMember>> GetByProjectIdAsync(Guid projectId);

        Task<ProjectMember?> GetMemberByUserIdAndProjectIdAsync(Guid userId, Guid projectId);

        void Update(ProjectMember member);

        void Delete(ProjectMember member);
        Task<bool> IsExistAsync(Guid userId, Guid projectId);
    
    }
}