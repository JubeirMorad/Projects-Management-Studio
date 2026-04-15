using Microsoft.EntityFrameworkCore;
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


        //
        //
        public async Task AddAsync(ProjectMember member)
        {
            await _context.ProjectMembers.AddAsync(member);
            await _context.SaveChangesAsync();
        }


        //
        //
        public async Task<ProjectMember?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectMembers.FirstOrDefaultAsync(m => m.Id == id);
        }


        //
        //
        public async Task<List<ProjectMember>?> GetByProjectIdAsync(Guid projectId)
        {
            return await _context.ProjectMembers.Where(m => m.ProjectId == projectId).ToListAsync();
        }

        //
        //
        public async Task<List<ProjectMember>?> GetByUserIdAsync(Guid userId)
        {
            return await _context.ProjectMembers.Where(m => m.UserId == userId).ToListAsync();
        }


        //
        //
        public async Task<ProjectMember?> GetMemberByUserIdAndProjectIdAsync(Guid userId, Guid projectId)
        {
            return await _context.ProjectMembers.FirstOrDefaultAsync(m => m.UserId == userId && m.ProjectId == projectId);
        }

        //
        //
        public async Task UpdateAsync(ProjectMember member)
        {
            _context.ProjectMembers.Update(member);

            await _context.SaveChangesAsync();
        }

    }
}