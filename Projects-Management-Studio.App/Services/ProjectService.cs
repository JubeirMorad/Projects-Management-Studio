
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _projectRepo = projectRepository;
            _userRepo = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddNewProjectAsync(string name, string? description, Guid ownerId)
        {
            if (await _projectRepo.GetByNameAsync(name) is not null)
                throw new Exception("project's name is already exist.");

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

            _projectRepo.Add(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Project>> GetMyProjectsAsync(Guid userId)
        {
            return await _projectRepo.GetProjectsByUserIdAsyn(userId);
        }


        public async Task DeleteAsync(Guid currentUserId, Guid projectId)
        {

            var project = await _projectRepo.GetByIdAsync(projectId);

            if (project is null)
                throw new Exception("project not found.");

            if (currentUserId != project.OwnerId)
                throw new Exception("you have no permission to delete this project.");

            _projectRepo.Delete(project);
            await _unitOfWork.SaveChangesAsync();   
            
        }

        
    }
}