using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.App.DTOs.Projects;
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
        public void Add(Project project)
        {
            _context.Projects.Add(project);
        }



        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<Project?> GetByNameAsync(string name)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Name == name);
        }



        public async Task<List<GetProjectDto>> GetProjectsByUserIdAsyn(Guid userId)
        {
            return await _context.Projects.AsNoTracking().Where(
                    p => p.OwnerId == userId ||
                    p.Members.Any(m => m.UserId == userId)
                    )
                    .Select(
                        p => new GetProjectDto (
                            p.Id,
                            p.Name,
                            p.Description,
                            p.OwnerId == userId,
                            p.Members.Count()
                        )
                    )
                    .ToListAsync();
        }


        public void Delete(Project project)
        {
            _context.Remove(project);
        }

        
        
    }
}