
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IMemberRepository _memberRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IProjectRepository projectRepository, IMemberRepository memberRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _projectRepo = projectRepository;
            _memberRepo = memberRepository;
            _userRepo = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddNewProjectAsync(string name, string? description, Guid ownerId)
        {
            if (await _projectRepo.GetByNameAsync(name) is not null)
                throw new Exception ("project's name is already exist.");

            User? user = await _userRepo.GetUserByIdAsync(ownerId);

            if (user is null)
                throw new Exception("user with this id not found.");


            Project project = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                OwnerId = ownerId 
            };
            

            ProjectMember projectMember = new()
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                UserId = ownerId,
                Role = "Manager"
            };

            _memberRepo.Add(projectMember); // without save changes
            
            _projectRepo.Add(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Project>?> GetProjectsByOwnerIdAsync(Guid ownerId)
        {
            return await _projectRepo.GetByOwnerIdAsync(ownerId);
        }
    }
}