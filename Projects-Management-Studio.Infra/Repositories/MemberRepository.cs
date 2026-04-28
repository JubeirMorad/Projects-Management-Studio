using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.App.DTOs.ProjectMembers;
using Projects_Management_Studio.App.DTOs.Projects;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Infra.Data;

namespace Projects_Management_Studio.Infra.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }




        public void Add(ProjectMember member)
        {
            _context.ProjectMembers.Add(member);
        }


        //
        //
        public void Delete(ProjectMember member)
        {
            _context.ProjectMembers.Remove(member);
        }


        //
        //
        public async Task<ProjectMember?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectMembers.FirstOrDefaultAsync(m => m.Id == id);
        }


        //
        //
        public async Task<List<GetMemberByProjectDto>> GetByProjectIdAsync(Guid projectId, Guid currentUserId)
        {
            return await _context.ProjectMembers.AsNoTracking()
                        .Where(m => m.ProjectId == projectId)
                        .Select( m => new GetMemberByProjectDto(
                            m.UserId,
                            m.User.Username,
                            m.Role.ToString(),
                            m.UserId == currentUserId
                        ))
                        .ToListAsync();
        }

        //
        //
        public async Task<List<GetMemberByUserDto>> GetByUserIdAsync(Guid userId)
        {
            return await _context.ProjectMembers
                        .AsNoTracking()
                        .Where(m => m.UserId == userId)
                        .Select(m => new GetMemberByUserDto(
                            m.ProjectId,
                            m.Project.Name,
                            m.Project.Description,
                            m.User.Tasks.Count(t => t.ProjectId == m.ProjectId),
                            m.Role.ToString(),
                            m.Project.OwnerId,
                            m.Project.Owner.Username
                        ))
                        .ToListAsync();
        }


        //
        //
        public async Task<ProjectMember?> GetMemberByUserIdAndProjectIdAsync(Guid userId, Guid projectId)
        {
            return await _context.ProjectMembers.AsNoTracking().FirstOrDefaultAsync(m => m.UserId == userId && m.ProjectId == projectId);
        }


        //
        //
        public async Task<bool> IsExistAsync(Guid userId, Guid projectId)
        {
            return await _context.ProjectMembers.AsNoTracking().AnyAsync(m => m.UserId == userId && m.ProjectId == projectId);
        }

        //
        //
        public void Update(ProjectMember member)
        {
            _context.ProjectMembers.Update(member);
        }

    }
}