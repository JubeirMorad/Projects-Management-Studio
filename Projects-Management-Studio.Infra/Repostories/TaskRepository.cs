using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task AddAsync(TaskItem taskItem)
        {
            await _context.Tasks.AddAsync(taskItem);
        }

        //
        public async Task<List<TaskItem>?> GetTasksByProjectIdAsync(Guid projectId)
        {
            return await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        //
        public async Task<List<TaskItem>?> GetTasksByUserIdAsync(Guid? userId)
        {
            return await _context.Tasks
                        .Where(t => t.AssignedToUserId == userId)
                        .ToListAsync();
        }
    }
}