
using System.Security.Cryptography;
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
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepo = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }


        //
        //
        //
        public async Task<(string token, string refreshToken)> LoginAsync(string email, string password)
        {

            User user = await _userRepo.GetUserByEmailAsync(email)
                        ?? throw new Exception("Invalid credentials");


            if (!_passwordHasher.Verify(user.PasswordHash, password))
                throw new Exception("Invalid credentials");


            string accessToken = _jwtProvider.GenerateToken(user);
            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userRepo.UpdateUserAsync(user);

            return (accessToken, refreshToken);

        }

        //
        //
        //
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