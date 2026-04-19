
using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Infra.Data;

namespace Projects_Management_Studio.Infra.Repostories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        //
        public void Add(TaskItem taskItem)
        {
             _context.Tasks.Add(taskItem);
        }

        //
        public async Task<List<TaskItem>> GetTasksByProjectIdAsync(Guid projectId)
        {
            return await _context.Tasks
                        .AsNoTracking()
                        .Where(t => t.ProjectId == projectId).ToListAsync();
        }

        //
        public async Task<List<TaskItem>> GetTasksByUserIdAsync(Guid? userId)
        {
            return await _context.Tasks
                        .AsNoTracking()
                        .Where(t => t.AssignedToUserId == userId)
                        .ToListAsync();
        }

        //
        public async Task<TaskItem?> GetByIdAsync(Guid taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        //
        public void Update(TaskItem taskItem)
        {
            _context.Tasks.Update(taskItem);
        }

        public Task<List<TaskItem>> GetTasksByUserIdAndProjectIdAsync(Guid userId, Guid projectId)
        {
            return _context.Tasks
                    .AsNoTracking()
                    .Where(t => t.AssignedToUserId == userId && t.ProjectId == projectId)
                    .ToListAsync();
        }


        //
        //
        public void UpdateRange(IEnumerable<TaskItem> tasks)
        {
            _context.Tasks.UpdateRange(tasks);
        }
    }
}