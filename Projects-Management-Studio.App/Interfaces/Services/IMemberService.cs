using Projects_Management_Studio.App.DTOs.ProjectMembers;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.App.Interfaces.Services
{
    public interface IMemberService
    {

        Task CreateMemberAsync(Guid currentUserId, Guid projectId, Guid userId, ProjectRole Role); // current user must be owner of the project

        Task<ProjectMember?> GetMemberByIdAsync(Guid memberId);

        Task<List<GetMemberByProjectDto>> GetProjectMembersAsync(Guid userId, Guid projectId);

        Task<List<GetMemberByUserDto>> GetUserMembersAsync(Guid userId);

        Task UpdateMemberAsync(Guid ownerId, Guid projectId, Guid userId, ProjectRole newRole);

        Task DeleteMemberAsync(Guid currentUserId, Guid userId, Guid projectId);

    }
}