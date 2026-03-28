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

        public async Task<Project?> GetByNameAsync(string name)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Name == name);
        }
    }
}