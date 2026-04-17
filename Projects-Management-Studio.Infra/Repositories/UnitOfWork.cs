
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.Infra.Data;

namespace Projects_Management_Studio.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context; 
        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}