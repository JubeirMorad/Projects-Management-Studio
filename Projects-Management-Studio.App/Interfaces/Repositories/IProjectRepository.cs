using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects_Management_Studio.App.DTOs.Projects;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        void Add(Project project);
        Task<Project?> GetByNameAsync(string name);
        Task<Project?> GetByIdAsync(Guid projectId);
        Task<List<GetProjectDto>> GetProjectsByUserIdAsyn(Guid userId);
        void Delete(Project project);
    }
}