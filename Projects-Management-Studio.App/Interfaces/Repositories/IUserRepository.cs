
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}