
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepo;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepo = projectRepository;
        }

        public async Task AddNewProjectAsync(string name, string? description, Guid ownerId)
        {
            if (await _projectRepo.GetByNameAsync(name) is not null)
                throw new Exception ("project's name is already exist.");


            Project project = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                OwnerId = ownerId 
            };

            await _projectRepo.AddAsync(project);
        }
    }
}