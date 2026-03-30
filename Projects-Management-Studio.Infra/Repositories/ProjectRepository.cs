using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Infra.Data;

namespace Projects_Management_Studio.Infra.Repostories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            _context.SaveChanges();
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<Project?> GetByNameAsync(string name)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<Project>?> GetByOwnerIdAsync(Guid ownerId)
        {
            List<Project>? projects = await _context.Projects.Where(p => p.OwnerId == ownerId)
                                                             .ToListAsync();
            
            return projects;
        }
    }
}