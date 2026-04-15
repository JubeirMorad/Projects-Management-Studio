using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Services
{
    public interface IMemberService
    {

        Task CreateMemberAsync(Guid currentUserId, Guid projectId, Guid userId); // current user must be owner of the project

        Task<ProjectMember?> GetMemberByIdAsync(Guid memberId);

        Task<List<ProjectMember>?> GetProjectMembersAsync(Guid userId, Guid projectId);

        Task<List<ProjectMember>?> GetUserMembersAsync(Guid userId);

        Task UpdateMemberAsync(Guid ownerId, Guid memberId, Guid projectId, Guid userId, string role);

    }
}