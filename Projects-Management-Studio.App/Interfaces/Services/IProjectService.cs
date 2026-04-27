
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Services
{
    public interface IProjectService
    {
        Task AddNewProjectAsync(string name, string? description, Guid ownerId);

        Task<List<Project>> GetMyProjectsAsync(Guid userId);
        Task DeleteAsync(Guid currentUserId , Guid projectId);
    }
}