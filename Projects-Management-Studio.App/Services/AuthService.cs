
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.App.Interfaces.Services;
using Projects_Management_Studio.App.Services.Interfaces;
using Projects_Management_Studio.Domain.Entities;

namespace Projects_Management_Studio.App.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepository;
            _passwordHasher = passwordHasher;
        }
        

        
        public async Task RegisterAsync(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.Hash(password);
            var user = new User()
            {
                Username = userName,
                Email = email,
                PasswordHash = hashedPassword
            };

            await _userRepo.AddUserAsync(user);
        }
    }
}