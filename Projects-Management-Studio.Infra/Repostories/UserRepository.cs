
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.Domain.Entities;
using Projects_Management_Studio.Infra.Data;

namespace Projects_Management_Studio.Infra.Repostories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;


        public UserRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        //
        //
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }

        //
        //
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        //
        //
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        //
        //
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}