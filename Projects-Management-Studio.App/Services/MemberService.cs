using Projects_Management_Studio.App.DTOs.ProjectMembers;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Domain.Enums;

namespace Projects_Management_Studio.App.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _taskRepo;

        public MemberService(IMemberRepository memberRepository, IProjectRepository projectRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, ITaskRepository taskRepository)
        {
            _memberRepo = memberRepository;
            _projectRepo = projectRepository;
            _userRepo = userRepository;
            _unitOfWork = unitOfWork;
            _taskRepo = taskRepository;
        }

        public async Task CreateMemberAsync(Guid currentUserId, Guid projectId, Guid userId, ProjectRole role) // current user must be owner of the project
        {

            // check for valid role
            if(!Enum.IsDefined(typeof(ProjectRole), role))
                throw new Exception("Invalid project role.");



            if ( await _projectRepo.GetByIdAsync(projectId) is not Project project)
                throw new Exception("Project does not exist.");


            // note : The owner is not considered a member of the project. => if current user is owner, currentMember = null 
            var currentMember = await _memberRepo.GetMemberByUserIdAndProjectIdAsync(currentUserId, projectId); 


            // check if the current user is the owner of the project or admin
            if (project.OwnerId != currentUserId)
            {

                if (currentMember is null)
                    throw new Exception("You are not a member of the project.");

                if (currentMember.Role != ProjectRole.Admin)
                        throw new Exception("Only the project owner and project admin can add members.");
            }


            // check if the user try to add project owner as member
            if (project.OwnerId == userId)
                throw new Exception("The project owner is already a member of the project.");


            // check if the user try to add him self as member
            if ( userId == currentUserId)
                throw new Exception("connot set your self as member");


            // check if the user exists
            if (await _userRepo.GetUserByIdAsync(userId) is null)
                throw new Exception("User does not exist.");


            // check if the user exists
            if (await _memberRepo.IsExistAsync(userId, projectId))
                throw new Exception("User is already a member of the project.");


            // check if member is admin and current user is also admin, only owner can assign admin role
            if (role == ProjectRole.Admin && currentMember?.Role == ProjectRole.Admin)
                throw new Exception("Only the project owner can assign admin role.");


            var member = new ProjectMember()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProjectId = projectId,
                Role = role 
            };

            _memberRepo.Add(member);
            await _unitOfWork.SaveChangesAsync();
            
        }



        //
        //
        //
        public async Task DeleteMemberAsync(Guid currentUserId, Guid userId, Guid projectId)
        {
            ProjectMember member =  await _memberRepo.GetMemberByUserIdAndProjectIdAsync(userId, projectId) ??
                throw new Exception("Project member not found.");


            // check if the project exists
            Project project = await _projectRepo.GetByIdAsync(projectId) ??
                throw new Exception("Project not found.");



            // check if the user exists

            User user = await _userRepo.GetUserByIdAsync(userId) ??
                throw new Exception("User not found.");


            // check if the current user is the owner of the project
            if (currentUserId != project.OwnerId)
                throw new Exception("Only the project owner can delete members.");

            if (member.UserId == currentUserId)
                throw new Exception("You cannot remove yourself from the project.");


            // update tasks 
            var tasks = await _taskRepo.GetTasksByUserIdAndProjectIdAsync(userId, projectId);

            foreach(TaskItem task in tasks)
            {
                task.AssignedToUserId = null;
            }

            _memberRepo.Delete(member);
            _taskRepo.UpdateRange(tasks);
            await _unitOfWork.SaveChangesAsync();
        }


        //
        //
        public async Task<ProjectMember?> GetMemberByIdAsync(Guid memberId)
        {
            return await _memberRepo.GetByIdAsync(memberId);
        }


        //
        //
        public async Task<List<GetMemberByProjectDto>> GetProjectMembersAsync(Guid userId, Guid projectId)
        {
            if (await _projectRepo.GetByIdAsync(projectId) is not Project project)
                throw new Exception("Project not found.");

            if (! await _memberRepo.IsExistAsync(userId, projectId) && userId != project.OwnerId)
                throw new Exception("You are not a member of the project.");

            return await _memberRepo.GetByProjectIdAsync(projectId, userId);
        }


        //
        //
        public async Task<List<ProjectMember>> GetUserMembersAsync(Guid userId)
        {
            return await _memberRepo.GetByUserIdAsync(userId);
        }


        //
        //
        public async Task UpdateMemberAsync(Guid ownerId, Guid projectId, Guid userId, ProjectRole newRole)
        {
            
            // check for valid role
            if(!Enum.IsDefined(typeof(ProjectRole), newRole))
                throw new Exception("Invalid project role.");


            User? user = await _userRepo.GetUserByIdAsync(userId);

            if (user is null)
                throw new Exception("user not found.");


            Project? project = await _projectRepo.GetByIdAsync(projectId);

            if (project is null)
                throw new Exception("project not found.");

            if (project.OwnerId == userId)
                throw new Exception("cannot update role of the project owner.");

            // check if the current user is the owner of the project
            if (ownerId != project.OwnerId)
                throw new Exception("you have no permmision to update this member.");

            ProjectMember? member = await _memberRepo.GetMemberByUserIdAndProjectIdAsync(userId, projectId);

            // check if the member exists
            if (member is null)
                throw new Exception("project member not found.");

            
            // check if role is the same
            if (member.Role == newRole)
                return;

            //update role
            member.Role = newRole;

            _memberRepo.Update(member);
            await _unitOfWork.SaveChangesAsync();
        }


        
    }
}