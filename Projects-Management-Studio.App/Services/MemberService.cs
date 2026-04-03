
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepo;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepo = memberRepository;
        }

        //
        public async Task<ProjectMember?> GetMemberByIdAsync(Guid memberId)
        {
            return await _memberRepo.GetByIdAsync(memberId);
        }


        //
        public async Task<List<ProjectMember>?> GetProjectMembersAsync(Guid userId, Guid projectId)
        {
            if (! await IsUserProjectMember(userId, projectId))
                throw new UnauthorizedAccessException("User is not a member of the project.");

            return await _memberRepo.GetByProjectIdAsync(projectId);
        }


        //
        public async Task<List<ProjectMember>?> GetUserMembersAsync(Guid userId)
        {
            return await _memberRepo.GetByUserIdAsync(userId);
        }






        //
        //
        //
        async Task<bool> IsUserProjectMember(Guid userId, Guid projectId)
        {
            var members = await _memberRepo.GetByProjectIdAsync(projectId);
            if (members == null) return false;

            return members.Any(m => m.UserId == userId);
        }
    }
}