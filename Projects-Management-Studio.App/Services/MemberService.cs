using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IUserRepository _userRepo;

        public MemberService(IMemberRepository memberRepository, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _memberRepo = memberRepository;
            _projectRepo = projectRepository;
            _userRepo = userRepository;
        }

        public async Task CreateMemberAsync(Guid currentUserId, Guid projectId, Guid userId) // current user must be owner of the project
        {

            if ( await _projectRepo.GetByIdAsync(projectId) is not Project project)
                throw new Exception("Project does not exist.");


            if (project.OwnerId != currentUserId)
                throw new UnauthorizedAccessException("Only the project owner can add members.");


            if (await _userRepo.GetUserByIdAsync(currentUserId) is null )
                throw new Exception("User does not exist.");


            if ( userId == currentUserId)
                throw new Exception("connot set your self as member");


            if (await IsUserProjectMember(userId, projectId))
                throw new Exception("User is already a member of the project.");



            var member = new ProjectMember()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProjectId = projectId
            };

            await _memberRepo.AddAsync(member);
            
        }


        //
        //
        public async Task<ProjectMember?> GetMemberByIdAsync(Guid memberId)
        {
            return await _memberRepo.GetByIdAsync(memberId);
        }


        //
        //
        public async Task<List<ProjectMember>?> GetProjectMembersAsync(Guid userId, Guid projectId)
        {
            if (! await IsUserProjectMember(userId, projectId))
                throw new UnauthorizedAccessException("You are not a member of the project.");

            return await _memberRepo.GetByProjectIdAsync(projectId);
        }


        //
        //
        public async Task<List<ProjectMember>?> GetUserMembersAsync(Guid userId)
        {
            return await _memberRepo.GetByUserIdAsync(userId);
        }


        //
        //
        public async Task UpdateMemberAsync(Guid ownerId, Guid memberId, Guid projectId, Guid userId, string role)
        {

            User? user = await _userRepo.GetUserByIdAsync(ownerId);

            if (user is null)
                throw new Exception("user not found.");


            Project? project = await _projectRepo.GetByIdAsync(projectId);

            if (project is null)
                throw new Exception("project not found.");

            if (userId != project.OwnerId)
                throw new Exception("you have no permmision to update this member.");

            ProjectMember? member = await _memberRepo.GetByIdAsync(memberId);

            if (member is null)
                throw new Exception("project member not found.");


            
            if (member.ProjectId == projectId && member.UserId == userId && member.Role == role)
                return;


            // update project id
            member.ProjectId = projectId;

            // update user id
            member.UserId = userId;

            //update role
            member.Role = role;

            await _memberRepo.UpdateAsync(member);
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