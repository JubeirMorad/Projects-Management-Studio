using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Services
{
    public interface IMemberService
    {
        Task<ProjectMember?> GetMemberByIdAsync(Guid memberId);

        Task<List<ProjectMember>?> GetProjectMembersAsync(Guid userId, Guid projectId);

        Task<List<ProjectMember>?> GetUserMembersAsync(Guid userId);

    }
}